namespace Hospital
{
    using System;
    using System.Data.Entity.Validation;
    using Models;

    class StartUp
    {
        static void Main(string[] args)
        {
            HospitalContext context = new HospitalContext();
            
            Patient patient = new Patient()
            {
                FirstName = "Ivan",
                LastName = "Ivanov",
                Address = "Mladost 1",
                Email = "abv@abv.bg",
                DateOfBirth = DateTime.Now
            };

            context.Patients.Add(patient);

            Visitation visitation = new Visitation()
            {
                Patient = patient,
                Date = DateTime.Now
            };

            context.Visitations.Add(visitation);

            Diagnose diagnose = new Diagnose()
            {
                Name = "Disease",
                Patient = patient
            };

            context.Diagnoses.Add(diagnose);

            Medicament medicament = new Medicament()
            {
                Name = "Vitamin C",
            };

            medicament.Patients.Add(patient);

            context.Medicaments.Add(medicament);

            Doctor doctor = new Doctor()
            {
                Name = "Dr. Ivanov",
                Specialty = "orthopedic doctor"
            };

            visitation.Doctor = doctor;

            context.Doctors.Add(doctor);

            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var ev in ex.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        ev.Entry.Entity.GetType().Name, ev.Entry.State);
                    foreach (var ve in ev.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
        }
    }
}
