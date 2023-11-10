namespace VacationManagement
{
    class Program
    {
        static void Main(string[] args)
        {
            var vacationDictionary = new Dictionary<string, List<DateTime>>();

            var employeeNames = new List<string>
            {
                "Иванов Иван Иванович",
                "Петров Петр Петрович",
                "Юлина Юлия Юлиановна",
                "Сидоров Сидор Сидорович",
                "Павлов Павел Павлович",
                "Георгиев Георг Георгиевич"
            };

            var workingDaysOfWeek = new List<DayOfWeek> { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday };

            const int maxVacationDaysPerYear = 28;

            GenerateVacations(vacationDictionary, employeeNames, workingDaysOfWeek, maxVacationDaysPerYear);

            foreach (var employee in vacationDictionary)
            {
                Console.WriteLine("Дни отпуска " + employee.Key + " : ");
                var vacationDates = employee.Value;
                int vacationDuration = 1;
                for (int i = 0; i < vacationDates.Count; i++)
                {
                    var startDate = vacationDates[i];
                    while (i + 1 < vacationDates.Count && (vacationDates[i + 1] - vacationDates[i]).Days == 1)
                    {
                        i++;
                        vacationDuration++;
                    }
                    var endDate = vacationDates[i].AddDays(vacationDuration - 1);
                    Console.WriteLine($"{startDate.ToShortDateString()} - {endDate.ToShortDateString()} ({vacationDuration} дней)");
                    vacationDuration = 1;
                }
            }
            Console.ReadKey();
        }

        static void GenerateVacations(Dictionary<string, List<DateTime>> vacationDictionary, List<string> employeeNames, List<DayOfWeek> workingDays, int maxVacationDaysPerYear)
        {
            var random = new Random();

            foreach (var employeeName in employeeNames)
            {
                var vacationList = new List<DateTime>();
                int totalVacationDays = 0;
                while (totalVacationDays < maxVacationDaysPerYear)
                {
                    var startDate = GenerateRandomWorkingDay(random, workingDays);

                    var maxVacationDuration = maxVacationDaysPerYear - totalVacationDays;
                    var vacationDuration = random.Next(2) == 0 ? 7 : 14;
                    if (vacationDuration > maxVacationDuration)
                    {
                        vacationDuration = maxVacationDuration;
                    }

                    if (IsVacationPossible(vacationList, startDate, vacationDuration))
                    {
                        for (int day = 0; day < vacationDuration; day++)
                        {
                            vacationList.Add(startDate.AddDays(day));
                        }
                        totalVacationDays += vacationDuration;
                    }
                }

                if (totalVacationDays > maxVacationDaysPerYear)
                {
                    var lastVacation = vacationList.Last();
                    var remainingDays = totalVacationDays - maxVacationDaysPerYear;
                    var newEndDate = lastVacation.AddDays(-remainingDays + 1);
                    vacationList[vacationList.Count - 1] = newEndDate;
                }

                vacationDictionary[employeeName] = vacationList;
            }
        }

        static DateTime GenerateRandomWorkingDay(Random random, List<DayOfWeek> workingDays)
        {
            DateTime date;
            do
            {
                date = DateTime.Today.AddDays(random.Next(365));
            } while (!workingDays.Contains(date.DayOfWeek));

            return date;
        }

        static bool IsVacationPossible(List<DateTime> vacationList, DateTime startDate, int duration)
        {
            var endDate = startDate.AddDays(duration - 1);
            foreach (var date in vacationList)
            {
                if (date >= startDate && date <= endDate)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
