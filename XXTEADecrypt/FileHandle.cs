using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class FileHandle{
    public List<string> fileBox = new List<string>();
    public string[] strFormat = new string[5];
    public XXTEADecrypt.Form1 mForm1;
    public string outputPath;
    public string inputPath;

    public FileHandle(XXTEADecrypt.Form1 mform,string inputPath, string outputPath)
    {
        this.mForm1 = mform;
        this.outputPath = outputPath;
        this.inputPath = inputPath;
    }

	public byte[] FileRead(string path)
	{
        try
        {
            FileInfo fi = new FileInfo(path);
            long len = fi.Length;
            FileStream fs = new FileStream(path, FileMode.Open);
            byte[] buffer = new byte[len];
            fs.Read(buffer, 0, (int)len);
            fs.Flush();
            fs.Close();
            return buffer;
        }
        catch (Exception e)
        {
            mForm1.richTextBox_log.AppendText("���ļ�����--->" + path + "\n");
            byte[] a = new byte[3];
            return a;
        }

        
	}
    public bool FileWrite(byte[] data, string pathAndName)
    {
        try
        {
            FileStream fs = new FileStream(pathAndName, FileMode.Create);
            fs.Write(data, 0, data.Length);
            fs.Flush();
            fs.Close();
        }
        catch (Exception e)
        {
            mForm1.richTextBox_log.AppendText("д��ʧ��--->" + pathAndName + "\n");
            return false;
        }

        
        return true;
    }
    public void FileToDirctory(string filePath)
    {
        FileInfo fi = new FileInfo(filePath);
        Console.WriteLine(fi.Directory.ToString());
        if (!Directory.Exists(outputPath))
        {
            Directory.CreateDirectory(outputPath);
        }
    }
	/// <summary>
    /// ��ȡָ��Ŀ¼�µ����з����������ļ��ľ���·��
    /// </summary>
    /// <param name="inputfolder">�����Ŀ¼</param>
    /// <param name="suffix">��������</param>
    /// <returns>�����������ļ��ľ���·���ַ�������</returns>
    public void DirectoryToFile(string inputFolder)
	{
		DirectoryInfo TheFolder = new DirectoryInfo(inputFolder);
		string absoultPath;
        //��ȡ��ǰĿ¼���ļ�
        if (strFormat[0].Equals(".*"))
        {
            foreach (FileInfo nextFile in TheFolder.GetFiles())
            {
                absoultPath = nextFile.FullName;
                fileBox.Add(absoultPath);

                Console.WriteLine("��ȡ���ļ�:" + absoultPath);
                mForm1.richTextBox_log.AppendText("�����ļ�--->" + absoultPath + "\n");
            }
        }
        else
        {
            foreach (FileInfo nextFile in TheFolder.GetFiles())
            {
                string tmp = nextFile.Name;
                for (int i = 0; strFormat[i] != null; i++)
                {
                    if (StrMatching(tmp, strFormat[i]))
                    {
                        absoultPath = nextFile.FullName;
                        fileBox.Add(absoultPath);
                        Console.WriteLine("��ȡ���ļ�:" + absoultPath);
                        mForm1.richTextBox_log.AppendText("�����ļ�--->" + absoultPath + "\n");
                        break;
                    }  
                }        
            }
        }
        string tmp2;
		foreach(DirectoryInfo nextFolder in TheFolder.GetDirectories())
		{
            tmp2 = nextFolder.FullName;
            tmp2 = tmp2.Replace(inputPath, outputPath);
			Console.WriteLine("��ȡ����Ŀ¼:" + nextFolder.FullName + ",��д��Ŀ¼:" + tmp2);
            if (!Directory.Exists(tmp2))
            {
                Directory.CreateDirectory(tmp2);
            }
            this.DirectoryToFile(nextFolder.FullName);
		}
	}
    
    /// <summary>
    /// ��ͨ����е�*��?ת��Ϊ������ʽ
    /// </summary>
    /// <param name="strWildcard">��ת���ַ���</param>
    /// <returns>ת���Ľ��</returns>
    public string WildcardToRegex(string strWildcard)
    {
        strWildcard = strWildcard.Replace(".", @"\.");
        strWildcard = strWildcard.Replace("?", @".[1]");
        strWildcard = strWildcard.Replace("*", @".*");
       // strWildcard = "^" + strWildcard;
        return strWildcard;
    }

    public bool StrMatching(string src, string strRule)
    {
        return Regex.IsMatch(src, strRule);
    }
    
}