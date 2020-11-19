using System.IO;
using System;
using System.Diagnostics.Eventing.Reader;


namespace FileSorting
{
    class Files
    {
        public string Origin { get; private set; }
        public string Destination { get; private set; }
        private string LogFile { get; set; }

        public double SuccessfulMoves { get; private set; }

        public double FailedMoves { get; private set; }

        private readonly string[] _fileList;
                

        public Files(string origin, string destination)
        {
            if (string.IsNullOrWhiteSpace(origin) || !Directory.Exists(origin) || string.IsNullOrWhiteSpace(destination) || !Path.IsPathRooted(destination))
                throw new InvalidOperationException("Both origin and destination paths must have valid formats."); // error if file paths are not valid

            Origin = origin;
            Destination = destination;
            LogFile = "log.txt";
            
            _fileList = Directory.GetFiles(origin); // creates a list of files by their entire filepath inc. filename ex. C:\...\origin\doc.txt
            
        }

        public string MoveFiles()
        {
            SuccessfulMoves = 0;
            FailedMoves = 0;

            foreach (string filepath in _fileList)
            {
                string filename = Path.GetFileName(filepath);
                
                if (filename != LogFile)
                {
                    string creationTime = File.GetCreationTime(filepath).ToString("yyyy-MM"); // this will be the destination folder name
                    string destDir = $"{Destination}\\{ creationTime}";                       // destination directory path not including filename
                    string destPath = $"{destDir}\\{filename}";            // destination path including filename ex. C:\...\dest\doc.txt

                    if (!Directory.Exists(destDir))         // create the destination directory if it doesn't exist yet
                        Directory.CreateDirectory(destDir); 

                    if (File.Exists(destPath))
                    {
                        LogError($"{DateTime.Now} | Operation failed on {filepath}.  File already exists in {destDir}."); // log an error with the current time, original filepath, and target destination dir.
                        FailedMoves++;
                    }
                    else
                    {
                        File.Move(filepath, destPath);          // move the file from the original to destination location
                        SuccessfulMoves++;
                    }

                }
                
            }

            DateTime endTime = DateTime.Now;

            LogError($"{endTime} | Total files Moved: {SuccessfulMoves} | Total files unmoved: {FailedMoves}"); // create a log entry once all files are moved indicating end time, no. files moved, no. failed moves

            return $"Task completed at {endTime}.  {SuccessfulMoves} files moved and {FailedMoves} files were not moved.  See log.txt in {Origin} for details."; // message of end time, no. files moved, no. failed moves

        }

        public void LogError(string error)
        {
            string logFilePath = $"{Origin}\\{LogFile}";

            using (System.IO.StreamWriter logStream = new System.IO.StreamWriter(logFilePath, true)) // add to existing log file else create new and add log
                logStream.WriteLine(error);

        }

    }
}
