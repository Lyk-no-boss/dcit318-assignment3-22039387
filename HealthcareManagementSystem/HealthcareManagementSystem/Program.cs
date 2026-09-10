using System;
using System.Collections.Generic;
using System.Linq;

namespace HealthcareManagementSystem
{
    // =========================================================
    // a. Generic Repository
    // =========================================================

    public class Repository<T>
    {
        private List<T> items = new List<T>();

        // Add an item
        public void Add(T item)
        {
            items.Add(item);
        }

        // Get all items
        public List<T> GetAll()
        {
            return items;
        }

        // Get an item by condition
        public T? GetById(Func<T, bool> predicate)
        {
            return items.FirstOrDefault(predicate);
        }

        // Remove an item by condition
        public bool Remove(Func<T, bool> predicate)
        {
            T? item = items.FirstOrDefault(predicate);

            if (item != null)
            {
                items.Remove(item);
                return true;
            }

            return false;
        }
    }


    // =========================================================
    // b. Patient Class
    // =========================================================

    public class Patient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }

        public Patient(int id, string name, int age, string gender)
        {
            Id = id;
            Name = name;
            Age = age;
            Gender = gender;
        }
    }


    // =========================================================
    // c. Prescription Class
    // =========================================================

    public class Prescription
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public string MedicationName { get; set; }
        public DateTime DateIssued { get; set; }

        public Prescription(
            int id,
            int patientId,
            string medicationName,
            DateTime dateIssued)
        {
            Id = id;
            PatientId = patientId;
            MedicationName = medicationName;
            DateIssued = dateIssued;
        }
    }


    // =========================================================
    // g. HealthSystemApp
    // =========================================================

    public class HealthSystemApp
    {
        private Repository<Patient> _patientRepo;
        private Repository<Prescription> _prescriptionRepo;

        private Dictionary<int, List<Prescription>> _prescriptionMap;


        // Constructor
        public HealthSystemApp()
        {
            _patientRepo = new Repository<Patient>();
            _prescriptionRepo = new Repository<Prescription>();

            _prescriptionMap =
                new Dictionary<int, List<Prescription>>();
        }


        // =====================================================
        // SeedData()
        // =====================================================

        public void SeedData()
        {
            // Add patients

            _patientRepo.Add(
                new Patient(1, "Kwame Mensah", 30, "Male")
            );

            _patientRepo.Add(
                new Patient(2, "Ama Boateng", 25, "Female")
            );

            _patientRepo.Add(
                new Patient(3, "Kofi Asante", 45, "Male")
            );


            // Add prescriptions

            _prescriptionRepo.Add(
                new Prescription(
                    1,
                    1,
                    "Paracetamol",
                    new DateTime(2026, 9, 1)
                )
            );

            _prescriptionRepo.Add(
                new Prescription(
                    2,
                    1,
                    "Amoxicillin",
                    new DateTime(2026, 9, 2)
                )
            );

            _prescriptionRepo.Add(
                new Prescription(
                    3,
                    2,
                    "Ibuprofen",
                    new DateTime(2026, 9, 3)
                )
            );

            _prescriptionRepo.Add(
                new Prescription(
                    4,
                    2,
                    "Vitamin C",
                    new DateTime(2026, 9, 4)
                )
            );

            _prescriptionRepo.Add(
                new Prescription(
                    5,
                    3,
                    "Cough Syrup",
                    new DateTime(2026, 9, 5)
                )
            );
        }


        // =====================================================
        // BuildPrescriptionMap()
        // =====================================================

        public void BuildPrescriptionMap()
        {
            _prescriptionMap.Clear();

            foreach (Prescription prescription
                     in _prescriptionRepo.GetAll())
            {
                if (!_prescriptionMap.ContainsKey(prescription.PatientId))
                {
                    _prescriptionMap[prescription.PatientId] =
                        new List<Prescription>();
                }

                _prescriptionMap[prescription.PatientId]
                    .Add(prescription);
            }
        }


        // =====================================================
        // d, e and f. Get Prescriptions by Patient ID
        // =====================================================

        public List<Prescription> GetPrescriptionsByPatientId(
            int patientId)
        {
            if (_prescriptionMap.ContainsKey(patientId))
            {
                return _prescriptionMap[patientId];
            }

            return new List<Prescription>();
        }


        // =====================================================
        // PrintAllPatients()
        // =====================================================

        public void PrintAllPatients()
        {
            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("           ALL PATIENTS");
            Console.WriteLine("======================================");

            foreach (Patient patient in _patientRepo.GetAll())
            {
                Console.WriteLine(
                    $"ID: {patient.Id} | " +
                    $"Name: {patient.Name} | " +
                    $"Age: {patient.Age} | " +
                    $"Gender: {patient.Gender}"
                );
            }
        }


        // =====================================================
        // PrintPrescriptionsForPatient()
        // =====================================================

        public void PrintPrescriptionsForPatient(int patientId)
        {
            Patient? patient =
                _patientRepo.GetById(p => p.Id == patientId);

            Console.WriteLine();
            Console.WriteLine("======================================");
            Console.WriteLine("        PATIENT PRESCRIPTIONS");
            Console.WriteLine("======================================");

            if (patient == null)
            {
                Console.WriteLine("Patient not found.");
                return;
            }

            Console.WriteLine(
                $"Patient: {patient.Name} (ID: {patient.Id})"
            );

            Console.WriteLine();

            List<Prescription> prescriptions =
                GetPrescriptionsByPatientId(patientId);

            if (prescriptions.Count == 0)
            {
                Console.WriteLine("No prescriptions found.");
                return;
            }

            foreach (Prescription prescription in prescriptions)
            {
                Console.WriteLine(
                    $"Prescription ID: {prescription.Id}"
                );

                Console.WriteLine(
                    $"Medication: {prescription.MedicationName}"
                );

                Console.WriteLine(
                    $"Date Issued: {prescription.DateIssued:dd/MM/yyyy}"
                );

                Console.WriteLine("--------------------------------------");
            }
        }
    }


    // =========================================================
    // Main Application
    // =========================================================

    class Program
    {
        static void Main(string[] args)
        {
            // i. Instantiate HealthSystemApp

            HealthSystemApp app =
                new HealthSystemApp();


            // ii. Seed the data

            app.SeedData();


            // iii. Build the prescription map

            app.BuildPrescriptionMap();


            // iv. Print all patients

            app.PrintAllPatients();


            // v. Display prescriptions for Patient ID 1

            app.PrintPrescriptionsForPatient(1);


            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
