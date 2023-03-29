using System.Runtime.Intrinsics.Arm;

namespace M8.Task3
{
    internal class Program
    {
        static void Main(string[] args)
        {
            long weight = 0;
            string DirPath = "";
            do
            {
                Console.WriteLine("Введите путь до папки для очистки:");
                DirPath = @"" + Console.ReadLine();
            } while (CheckDir(DirPath));

            WeighDir(DirPath, ref weight);
            Console.WriteLine($"Размер заданой папки = {weight} байт");

            weight = 0;
            int count = 0;
            CleanDir(DirPath, ref weight, ref count);

            Console.WriteLine($"Общий объем удаленных файлов = {weight}. Всего удалено {count} файлов и папок");
        }

        static void CleanDir(string dp, ref long weight, ref int count)
        {
            foreach (var d in Directory.GetDirectories(dp))
            {
                if (DateTime.Now - System.IO.File.GetLastWriteTime(d) < TimeSpan.FromMinutes(30))
                {
                    foreach (var f in Directory.GetFiles(d))
                    {
                        long length = new System.IO.FileInfo(f).Length;
                        weight += length;
                        count ++;
                    }
                    count++;
                    Directory.Delete(d, true);
                }
                else { Console.WriteLine($"Папка {d} был модифицирована менее 30 минут назад"); }
            }

            foreach (var f in Directory.GetFiles(dp))
            {
                if (DateTime.Now - System.IO.File.GetLastWriteTime(f) < TimeSpan.FromMinutes(30))
                {
                    long length = new System.IO.FileInfo(f).Length;
                    weight += length;
                    count++;
                    File.Delete(f);
                }
                else { Console.WriteLine($"Файл {f} был модифицирован менее 30 минут назад"); }
            }
        }

        static bool CheckDir(string dp)
        {
            if (dp != "")
            {
                if (Directory.Exists(dp))
                {
                    return false;
                }
                else { Console.WriteLine("Введенная папка не существует"); return true; }
            }
            else { return true; };
        }

        static void WeighDir(string dp, ref long weight)
        {
            foreach (var d in Directory.GetDirectories(dp))
            {
                WeighDir(d, ref weight);
            }

            foreach (var f in Directory.GetFiles(dp))
            {
                long length = new System.IO.FileInfo(f).Length;
                weight += length;
            }
            return;
        }
    }
}