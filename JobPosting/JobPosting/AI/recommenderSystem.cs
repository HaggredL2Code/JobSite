using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Diagnostics;

namespace JobPosting.AI
{
    public static class recommenderSystem
    {
        public static void FavoriteJobType_train(int user, int prev1, int prev2, int Y, int n_y, string userName)
        {
            // full path of python interpreter 
            string python = @"C:\Users\pevip\Anaconda3\python.exe";

            // python app to call 
            string myPythonApp = @"C:\Users\pevip\OneDrive\Documents\GitHub\JobSite\JobPosting\JobPosting\AI\RecommenderSystem\Train.py";


            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            // start python app with 3 arguments  
            // 1st arguments is pointer to itself,  
            // 2nd and 3rd are actual arguments we want to send 
            myProcessStartInfo.Arguments = myPythonApp + " " + user + " " + prev1 + " " + prev2 + " " + n_y + " " + Y + " " + userName;

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;


            // start the process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            /*if you need to read multiple lines, you might use: 
                string myString = myStreamReader.ReadToEnd() */
            Console.WriteLine(myString);
            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
            myProcess.Close();




        }

        public static int FavoriteJobType_predict(int user, int prev1, int prev2, string userName)
        {
            // full path of python interpreter 
            string python = @"C:\Users\pevip\Anaconda3\python.exe";

            // python app to call 
            string myPythonApp = @"C:\Users\pevip\OneDrive\Documents\GitHub\JobSite\JobPosting\JobPosting\AI\RecommenderSystem\predict.py";


            // Create new process start info 
            ProcessStartInfo myProcessStartInfo = new ProcessStartInfo(python);

            // make sure we can read the output from stdout 
            myProcessStartInfo.UseShellExecute = false;
            myProcessStartInfo.RedirectStandardOutput = true;

            // start python app with 3 arguments  
            // 1st arguments is pointer to itself,  
            // 2nd and 3rd are actual arguments we want to send 
            myProcessStartInfo.Arguments = myPythonApp + " " + user + " " + prev1 + " " + prev2 + " " + userName;

            Process myProcess = new Process();
            // assign start information to the process 
            myProcess.StartInfo = myProcessStartInfo;


            // start the process 
            myProcess.Start();

            // Read the standard output of the app we called.  
            // in order to avoid deadlock we will read output first 
            // and then wait for process terminate: 
            StreamReader myStreamReader = myProcess.StandardOutput;
            string myString = myStreamReader.ReadLine();

            /*if you need to read multiple lines, you might use: 
                string myString = myStreamReader.ReadToEnd() */
            int jobTypeID = Convert.ToInt32(myString);
            // wait exit signal from the app we called and then close it. 
            myProcess.WaitForExit();
            myProcess.Close();

            return jobTypeID;


        }
    }
}