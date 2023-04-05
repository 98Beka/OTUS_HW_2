using HW2;
using HW2.Model;
using System.Threading.Channels;

const string connectionString = "Server=127.0.0.1;Port=5432;Database=OTUS;User Id=postgres;Password=egatef;";

var script = new SqlScript(connectionString);
script.CreateStudentsTable();
script.CreateTutorsTable();
script.CreateLessonSubjectsTables();



while (true) {
    Console.Clear();
    Console.WriteLine("Показать данные таблиц: 1;   Ввести данные в таблицы: 2; Проинициализировать таблицы: 3;");
    Console.Write("->");
    var command = Console.ReadKey().KeyChar;
    switch(command) {
        case '1' :
            Console.Clear();
            ShowData();
            Console.WriteLine("кликните любую кнопку");
            Console.ReadKey();
            break;
        case '2' :
            Console.Clear();
            try {
                InsertData();
            } catch { Console.WriteLine("Неверный формат данных!!!"); }
            Console.WriteLine("кликните любую кнопку");
            Console.ReadKey();
            break;
        case '3' :
            Console.Clear();
            try { 
                script.InsertFiveRowsIntoTables();
                Console.WriteLine("Готово!!!");
            } catch { Console.WriteLine("таблицы уже проинициализированы!!!"); }
            Console.WriteLine("кликните любую кнопку");
            Console.ReadKey();
            break;
        default:
            Console.Clear();
            break;
    }
}

void InsertData() {
    while (true) {
        Console.WriteLine("В какую таблицу хотите добвать данные?");
        Console.WriteLine("Students: 1;   Tutors: 2;  LessonSubjects: 3");
        Console.Write("->");
        var command = Console.ReadKey().KeyChar;
        switch (command) {
            case '1':
                Console.Clear();

                Console.Write("first name: ");
                var firstName = Console.ReadLine();
                Console.Write("middle name: ");
                var middleName = Console.ReadLine();
                Console.Write("last name: ");
                var lastName = Console.ReadLine();
                int age = GetInt32("age: ");
                Console.Write("email: ");
                var email = Console.ReadLine();

                script.InsertDataForStuents(new StudentModel() {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Age = age,
                    Email = email
                });
                Console.WriteLine();
                Console.WriteLine("готово!!!");
                return;
            case '2':
                Console.Clear();

                Console.Write("first name: ");
                firstName = Console.ReadLine();
                Console.Write("middle name: ");
                middleName = Console.ReadLine();
                Console.Write("last name: ");
                lastName = Console.ReadLine();
                age = GetInt32("age: ");
                Console.Write("email: ");
                email = Console.ReadLine();

                script.InsertDataForTutors(new TutorModel() {
                    FirstName = firstName,
                    MiddleName = middleName,
                    LastName = lastName,
                    Age = age,
                    Email = email
                });
                Console.WriteLine();
                Console.WriteLine("готово!!!");
                return;
            case '3':
                Console.Clear();

                long studentId = GetInt64("studentId: ");
                long tutorId = GetInt64("tutorId: ");
                Console.Write("tytle: ");
                var tytle = Console.ReadLine();
                Console.Write("notes: ");
                var notes = Console.ReadLine();
                Console.Write("place: ");
                var place = Console.ReadLine();
                int points = GetInt32("points: ");
                Console.Write("result: ");
                var result = Console.ReadLine();


                script.InsertDataForLessonSubjects(new LessonSubjectModel() {
                    StudentId = studentId,
                    TutorId = tutorId,
                    Tytle = tytle,
                    Notes = notes,
                    Place = place,
                    Points = points,
                    Result = result
                });
                Console.WriteLine();
                Console.WriteLine("готово!!!");
                return;
            default:
                return;
        }
    }
}

int GetInt32(string message) {
    string res;
    while (true) {
        Console.Write(message);
        res = Console.ReadLine();
        if (Int32.TryParse(res, out int x))
            break;
        Console.WriteLine("введите число!!!");
    }
    return Int32.Parse(res);
}
long GetInt64(string message) {
    string res;
    while (true) {
        Console.Write(message);
        res = Console.ReadLine();
        if (Int64.TryParse(res, out long x))
            break;
        Console.WriteLine("введите число!!!");
    }
    return Int64.Parse(res);
}

void ShowData() {
    script.ReadData();
}
