using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DMT;

public class LocalisationTable
{
    private string[] columns;
    private Dictionary<string, string[]> lookup = new Dictionary<string, string[]>();


    public void Merge(LocalisationTable table)
    {
        foreach (var kv in table.lookup)
        {
            AddRow(table.columns, kv.Value);
        }
    }

    private int GetColumnIndex(string column)
    {
        for (int i = 0; i < columns.Length; i++)
        {
            if (columns[i] == column)
            {
                return i;
            }
        }
        return -1;
    }

    public void AddRow(string[] newColumns, string[] newRow)
    {
        if (newRow.Length != newColumns.Length)
        {
            throw new Exception("Column length mismatch");
        }

        // Try update existing row
        string[] row;
        if (lookup.TryGetValue(newRow[0], out row))
        {
            for (int i = 0; i < newColumns.Length; i++)
            {
                int columnIndex = GetColumnIndex(newColumns[i]);
                row[columnIndex] = newRow[i];
            }
            return;
        }

        // Create blank row
        row = new string[columns.Length];
        for (int j = 0; j < row.Length; j++)
        {
            row[j] = "";
        }

        // Copy valid newRow values
        for (int k = 0; k < newColumns.Length; k++)
        {
            int columnIndex = this.GetColumnIndex(newColumns[k]);
            row[columnIndex] = newRow[k];
        }

        lookup.Add(newRow[0], row);
    }

    public void Load(string path)
    {
        using (FileStream fileStream = File.OpenRead(path))
        {
            using (BinaryReader binaryReader = new BinaryReader(fileStream))
            {
                if (ReadColumns(binaryReader))
                {
                    Logging.LogWarning(path + " detected BOM in file");
                }
                ReadFromStream(binaryReader, path);
            }
        }
    }

    public void Save(string path)
    {
        using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        {
            using (StreamWriter streamWriter = new StreamWriter(fileStream))
            {
                WriteColumns(streamWriter);

                int rowIndex = 0;
                foreach (var kv in lookup)
                {
                    WriteRow(streamWriter, kv.Value);
                    if (rowIndex < lookup.Count - 1)
                    {
                        streamWriter.Write("\r\n");
                    }
                    rowIndex++;
                }
            }
        }
    }

    private bool ReadColumns(BinaryReader br)
    {

        var ret = false;
        byte[] bits = new byte[3];
        br.Read(bits, 0, 3);

        // UTF8 byte order mark is: 0xEF,0xBB,0xBF
        if (bits[0] == 0xEF && bits[1] == 0xBB && bits[2] == 0xBF)
        {
            ret = true;
        }
        else
        {
            br.BaseStream.Position -= 3;
        }


        StringBuilder buffer = new StringBuilder();
        while (br.BaseStream.Position < br.BaseStream.Length)
        {
            char c = br.ReadChar();
            if (c != '\r')
            {
                if (c == '\n')
                {
                    break;
                }
                buffer.Append(c);
            }
        }

        string row = buffer.ToString();
        columns = row.Split(',');
        return ret;
    }

    private void WriteColumns(StreamWriter sw)
    {
        for (int i = 0; i < columns.Length; i++)
        {
            sw.Write(columns[i]);
            if (i < columns.Length - 1)
            {
                sw.Write(',');
            }
        }
        sw.Write("\r\n");
    }

    private void WriteRow(StreamWriter sw, string[] rowValues)
    {
        if (rowValues == null || rowValues.Any(d=> d == null)) return;
        for (int i = 0; i < rowValues.Length; i++)
        {
            bool needsQuotes = rowValues[i].Contains(',');
            if (needsQuotes)
            {
                sw.Write('"');
            }
            sw.Write(rowValues[i]);
            if (needsQuotes)
            {
                sw.Write('"');
            }
            if (i < rowValues.Length - 1)
            {
                sw.Write(',');
            }
        }
    }

    private void ReadFromStream(BinaryReader br, string path)
    {
        StringBuilder cellBuffer = new StringBuilder();
        bool inQuotes = false;
        string[] rowValues = new string[columns.Length];
        int columnIndex = 0;
        int lineIndex = 1;

        while (br.BaseStream.Position < br.BaseStream.Length)
        {
            char c = br.ReadChar();
            if (c == '"')
            {
                inQuotes = !inQuotes;
            }
            else
            {
                if (c == ',')
                {
                    if (!inQuotes)
                    {
                        if (columnIndex >= rowValues.Length)
                        {
                            throw new Exception("'" + path + "' Row index mismatch on line: " + (lineIndex + 1) + ". Expected " + rowValues.Length + " but got " + (columnIndex + 1));
                        }
                        rowValues[columnIndex] = cellBuffer.ToString();
                        columnIndex++;
                        cellBuffer.Length = 0;
                        continue;
                    }
                }
                else
                {
                    if (c == '\r')
                    {
                        continue;
                    }

                    if (c == '\n' && !inQuotes)
                    {
                        if (columnIndex >= columns.Length)
                        {
                            throw new Exception("'" + path + "' Column mismatch on line: " + (lineIndex + 1) + ". Expected " + columns.Length + " but got " + (columnIndex + 1));
                        }

                        rowValues[columnIndex] = cellBuffer.ToString();
                        columnIndex = 0;
                        cellBuffer.Length = 0;
                        lineIndex++;

                        if (!lookup.ContainsKey(rowValues[0]))
                            lookup.Add(rowValues[0], rowValues);
                        rowValues = new string[columns.Length];
                        continue;
                    }
                }

                cellBuffer.Append(c);
            }

        }

        if (columnIndex >= columns.Length)
        {
            throw new Exception("'" + path + "' Column mismatch on line: " + (lineIndex + 1) + "\nExpected " + columns.Length + " but got " + (columnIndex + 1));
        }

        rowValues[columnIndex] = cellBuffer.ToString();
        lookup.Add(rowValues[0], rowValues);
    }
}