using System;
using System.Collections.Generic;
using System.IO;

namespace StudentGradingSystem
{
    // Student class
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Score { get; set; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100)
                return "A";
            else if (Score >= 70)
                return "B";
            else if (Score >= 60)
                return "C";
            else if (Score >= 50)
                return "D";
            else
                return "F";
        }
    }

    // Custom exception for invalid score format
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message)
            : base(message)
        {
        }
    }

    // Custom exception for missing fields
    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message)
            : base(message)
        {
        }
    }

    // Student result processor
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            List<Student> students = new List<Student>();

            using (StreamReader reader = new StreamReader(inputFilePath))
            {
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrWhiteSpace(line))
                        continue;

                    string[] fields = line.Split(',');

                    // Check that there are exactly 3 fields
                    if (fields.Length != 3)
                    {
                        throw new MissingFieldException(
                            $"Invalid record: {line}. Expected ID, Full Name, and Score."
                        );
                    }

                    // Check for missing values
                    if (string.IsNullOrWhiteSpace(fields[0]) ||
                        string.IsNullOrWhiteSpace(fields[1]) ||
                        string.IsNullOrWhiteSpace(fields[2]))
                    {
                        throw new MissingFieldException(
                            $"Missing field in record: {line}"
                        );
                    }

                    int id;

                    if (!int.TryParse(fields[0].Trim(), out id))
                    {
                        throw new InvalidScoreFormatException(
                            $"Invalid student ID: {fields[0]}"
                        );
                    }

                    int score;

                    if (!int.TryParse(fields[2].Trim(), out score))
                    {
                        throw new InvalidScoreFormatException(
                            $"Invalid score for student {fields[1].Trim()}: {fields[2]}"
                        );
                    }

                    if (score < 0 || score > 100)
                    {
                        throw new InvalidScoreFormatException(
                            $"Score must be between 0 and 100. Invalid score: {score}"
                        );
                    }

                    Student student = new Student(
                        id,
                        fields[1].Trim(),
                        score
                    );

                    students.Add(student);
                }
            }

            return students;
        }

        public void WriteReportToFile(
            List<Student> students,
            string outputFilePath)
        {
            using (StreamWriter writer = new StreamWriter(outputFilePath))
            {
                foreach (Student student in students)
                {
                    writer.WriteLine(
                        $"{student.FullName} (ID: {student.Id}): " +
                        $"Score = {student.Score}, Grade = {student.GetGrade()}"
                    );
                }
            }
        }
    }

    // Main application
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputFilePath = "students.txt";
                string outputFilePath = "student_report.txt";

                StudentResultProcessor processor =
                    new StudentResultProcessor();

                List<Student> students =
                    processor.ReadStudentsFromFile(inputFilePath);

                processor.WriteReportToFile(
                    students,
                    outputFilePath
                );

                Console.WriteLine(
                    "Student records processed successfully."
                );

                Console.WriteLine(
                    $"Report created: {outputFilePath}"
                );
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine(
                    "Error: The input file could not be found."
                );
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine(
                    $"Invalid Score Format Error: {ex.Message}"
                );
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine(
                    $"Missing Field Error: {ex.Message}"
                );
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"An unexpected error occurred: {ex.Message}"
                );
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
