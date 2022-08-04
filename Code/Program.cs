using System;
using System.Collections.Generic;
using System.IO;
namespace File_Ver_G_Code
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Record> records = new List<Record>();
            List<Tuple<int, int, int, int>> avail = new List<Tuple<int, int, int, int>>();
            Console.WriteLine("Choose a file name without .txt\nFile Names Are:\nEmployees1-before\nEmployees2-before\nEmployees3-before\nEmployees4-before\nEmployees5-before\n\n");
            string filename = Console.ReadLine() + ".txt";
            FileStream fs = new FileStream(filename, FileMode.Open);
            StreamReader sr = new StreamReader(fs);

            while (sr.Peek() != -1) {

                int id = int.Parse(sr.ReadLine());
                
                string name = sr.ReadLine();
                
                int phonenumber = int.Parse(sr.ReadLine());
                Record tmpRec = new Record();
                tmpRec.employee = new Employee();
                tmpRec.employee.id = id;
                tmpRec.employee.empname = name;
                tmpRec.employee.phonenumber = phonenumber;
                tmpRec.size = name.Length + 4 * 2;
                tmpRec.frag = 0;
                tmpRec.recname = "R" + records.Count;
                records.Add(tmpRec);
                Console.WriteLine("RecName: " + tmpRec.recname + "\tRecSize: " + tmpRec.size + "\tRecFrag: " + tmpRec.frag);
                Console.WriteLine("EmpId: " + tmpRec.employee.id + "\tEmpName: " + tmpRec.employee.empname + "\tEmpPhone: " + tmpRec.employee.phonenumber);
                Console.WriteLine("///////////////////////////////////////\n");

            }
            sr.Close();
            fs.Close();
            Console.WriteLine("File is loaded successfully\n\n");
            int counterofadds = records.Count;
            while (true) {
                Console.WriteLine("Enter 'a' to add, 'd' to delete, 's' to display and 'e' to exit and save in file");
                char add_or_delete_or_display_or_exit = Char.Parse(Console.ReadLine());
                if (add_or_delete_or_display_or_exit == 'a')
                {

                    Console.WriteLine("Enter Employee id:");
                    int id = int.Parse(Console.ReadLine());
                    Console.WriteLine("Enter Employee name:");
                    string name = Console.ReadLine();
                    Console.WriteLine("Enter Employee phone number:");
                    int phonenumber = int.Parse(Console.ReadLine());
                    Record tmpRec = new Record();
                    tmpRec.employee = new Employee();
                    tmpRec.employee.id = id;
                    tmpRec.employee.empname = name;
                    tmpRec.employee.phonenumber = phonenumber;
                    tmpRec.size = name.Length + 4 * 2;
                    tmpRec.recname = "R" + counterofadds;
                    counterofadds++;
                    if (avail.Count == 0)
                    {
                        tmpRec.frag = 0;
                        records.Add(tmpRec);

                    }
                    else
                    {
                        int i;
                        for (i = 0; i < avail.Count; i++)
                        {
                            if (avail[i].Item2 >= tmpRec.size)
                            {
                                int indexofrec = avail[i].Item4;
                                records[indexofrec] = tmpRec;
                                records[indexofrec].frag = avail[i].Item2 - tmpRec.size;
                                
                                break;
                            }

                        }
                        if (i == avail.Count)
                        {
                            tmpRec.frag = 0;
                            records.Add(tmpRec);

                        }
                        else {

                            avail.RemoveAt(i);

                        }
                        if(avail.Count > 0) { 
                            List<Tuple<int, int, int, int>> availtmp = new List<Tuple<int, int, int, int>>();
                            availtmp.Add(new Tuple<int, int, int, int>(avail[0].Item1, avail[0].Item2, -1, avail[0].Item4));
                            records[avail[0].Item4].recname = "*-1";
                            for (int x = 1; x <avail.Count; x++) {
                                availtmp.Add(new Tuple<int, int, int, int> (avail[x].Item1,avail[x].Item2,avail[x-1].Item1,avail[x].Item4) );
                                records[avail[x].Item4].recname = "*" + avail[x - 1].Item1;


                                // offset, size, pointer= bef, numberofrecordtobedeleted
                            }
                            avail.Clear();
                            avail = availtmp;
                        }

                    }
                }
                else if (add_or_delete_or_display_or_exit == 'd')
                {

                    Console.WriteLine("Choose number of record you would like to delete");// 0 to " + (records.Count - 1));
                    for (int x = 0; x < records.Count; x++) {

                        if (records[x].employee != null) {

                            Console.WriteLine(x);
                            
                        }
                        
                    }
                    Console.WriteLine();
                    int numberofrecordtobedeleted = int.Parse(Console.ReadLine());
                    if (avail.Count == 0)
                    {
                        records[numberofrecordtobedeleted].recname = "*-1";
                        records[numberofrecordtobedeleted].employee = null;
                        
                        int offset = 1, size = records[numberofrecordtobedeleted].size, pointer = -1;
                        for (int i = 0; i < numberofrecordtobedeleted; i++)
                        {
                            offset += records[i].size;
                        }
                        records[numberofrecordtobedeleted].frag = size;

                        Tuple<int, int, int, int> tmpTup = new Tuple<int, int, int, int>(offset, size, pointer, numberofrecordtobedeleted);
                        avail.Add(tmpTup);
                    }
                    else
                    {

                        records[numberofrecordtobedeleted].recname = "*" + avail[avail.Count - 1].Item1;
                        records[numberofrecordtobedeleted].employee = null;
                        int offset = 1, size = records[numberofrecordtobedeleted].size, pointer = avail[avail.Count - 1].Item1;
                        for (int i = 0; i < numberofrecordtobedeleted; i++)
                        {
                            offset += records[i].size;
                        }
                        records[numberofrecordtobedeleted].frag = size;

                        Tuple<int, int, int, int> tmpTup = new Tuple<int, int, int, int>(offset, size, pointer, numberofrecordtobedeleted);
                        avail.Add(tmpTup);
                    }

                }
                else if (add_or_delete_or_display_or_exit == 's')
                {
                    
                    Console.WriteLine("\nHere's the list of records:\n");
                    for (int i = 0; i < records.Count; i++)
                    {
                        Record tmp = records[i];
                        Console.WriteLine("RecName: " + tmp.recname + "\tRecSize: " + tmp.size + "\tRecFrag: " + tmp.frag);
                        Console.WriteLine("Contains Employee: ");
                        if (tmp.employee != null)
                        {
                            
                            Console.WriteLine("EmpId: " + tmp.employee.id + "\tEmpName: " + tmp.employee.empname + "\tEmpPhone: " + tmp.employee.phonenumber);
                        }
                        else
                        {

                            Console.WriteLine("Deleted");
                        }
                        Console.WriteLine("///////////////////////////////////////\n");
                    }
                    
                    Console.WriteLine("And here's the avail list:\n");
                    for (int i = 0; i < avail.Count; i++)
                    {
                        Console.WriteLine("Offset: " + avail[i].Item1 + "\tSize: " + avail[i].Item2 + "\tPointer: " + avail[i].Item3);

                        Console.WriteLine("///////////////////////////////////////\n");
                    }
                    if (avail.Count == 0) {

                        Console.WriteLine("Empty");

                        Console.WriteLine("///////////////////////////////////////\n");

                    }



                }
                else {
                    string[] filetmparr = filename.Split('-');
                    string filenameout = filetmparr[0] + "-after.txt";
                    string recordfile = filetmparr[0] + "-records-after.txt";
                    if (File.Exists(filenameout))
                        File.WriteAllText(filenameout, string.Empty);

                    FileStream fs2 = new FileStream(filenameout, FileMode.OpenOrCreate);
                    StreamWriter sw = new StreamWriter(fs2);

                    for (int i = 0; i < records.Count; i++)
                    {
                        Record tmp = records[i];
                        
                        if (tmp.employee != null)
                        {
                            sw.WriteLine(tmp.employee.id);
                            sw.WriteLine(tmp.employee.empname);
                            sw.WriteLine(tmp.employee.phonenumber);
                            
                        }
                        else
                        {
                            sw.WriteLine(tmp.recname);
                            sw.WriteLine(tmp.recname);
                            sw.WriteLine(tmp.recname);
                            
                        }
                    }
                    sw.Close();
                    fs2.Close();

                    if (File.Exists(recordfile))
                        File.WriteAllText(recordfile, string.Empty);

                    fs2 = new FileStream(recordfile, FileMode.OpenOrCreate);
                    sw = new StreamWriter(fs2);
                    sw.WriteLine("Here's the list of records:\n");
                    for (int i = 0; i < records.Count; i++)
                    {
                        Record tmp = records[i];
                        sw.WriteLine("RecName: " + tmp.recname + "\tRecSize: " + tmp.size + "\tRecFrag: " + tmp.frag);
                        sw.WriteLine("Contains Employee: ");
                        if (tmp.employee != null)
                        {

                            sw.WriteLine("EmpId: " + tmp.employee.id + "\tEmpName: " + tmp.employee.empname + "\tEmpPhone: " + tmp.employee.phonenumber);
                        }
                        else
                        {

                            sw.WriteLine("Deleted");
                        }
                        sw.WriteLine("///////////////////////////////////////\n");
                    }

                    sw.WriteLine("And here's the avail list:\n");
                    for (int i = 0; i < avail.Count; i++)
                    {
                        sw.WriteLine("Offset: " + avail[i].Item1 + "\tSize: " + avail[i].Item2 + "\tPointer: " + avail[i].Item3);

                        sw.WriteLine("///////////////////////////////////////\n");
                    }
                    if (avail.Count == 0)
                    {

                        sw.WriteLine("Empty");

                        sw.WriteLine("///////////////////////////////////////\n");

                    }
                    sw.Close();
                    fs2.Close();

                    Console.WriteLine("\nSaved Successfully in " + filenameout+" and in " + recordfile +"\nGoodbye!\n");

                    break;

                }
            }
        }
    }
}
