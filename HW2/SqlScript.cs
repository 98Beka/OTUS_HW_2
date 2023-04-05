using HW2.Model;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace HW2 {
    public class SqlScript {
        private readonly NpgsqlConnection _connection;
        public SqlScript(string connectionString) { 
            _connection = new NpgsqlConnection(connectionString);
        }

        public void CreateStudentsTable() {
            _connection.Open();

            var sql = @"
                CREATE SEQUENCE IF NOT EXISTS students_id_seq;

                CREATE TABLE IF NOT EXISTS students (
                    id                  BIGINT                  NOT NULL        DEFAULT NEXTVAL('students_id_seq'),
                    first_name          CHARACTER VARYING(255)  NOT NULL,
                    last_name           CHARACTER VARYING(255)  NOT NULL,
                    middle_name         CHARACTER VARYING(255)  NOT NULL,
                    email               CHARACTER VARYING(255)  NOT NULL,
                    age                 INTEGER                 NOT NULL,
                    
                    CONSTRAINT  students_pkey PRIMARY  KEY (id),
                    CONSTRAINT  students_email_unique  UNIQUE (email)
                    
                );
            ";

            using var cmd = new NpgsqlCommand(sql, _connection);
            var affectedRowsCount = cmd.ExecuteNonQuery().ToString();
            Console.WriteLine($"Created STUDENTS table. Affected rows count: {affectedRowsCount}");
        }
        public void CreateTutorsTable() {

            var slq = @"
                CREATE SEQUENCE IF NOT EXISTS tutors_id_seq;

                CREATE TABLE IF NOT EXISTS tutors (
                    id                  BIGINT                  NOT NULL        DEFAULT NEXTVAL('tutors_id_seq'),
                    first_name          CHARACTER VARYING(255)  NOT NULL,
                    middle_name         CHARACTER VARYING(255)  NOT NULL,
                    last_name           CHARACTER VARYING(255)  NOT NULL,
                    email               CHARACTER VARYING(255)  NOT NULL,
                    age                 INTEGER                 NOT NULL,
                    
                    CONSTRAINT  tutors_pkey PRIMARY  KEY (id)
                );
            ";
            using var cmd = new NpgsqlCommand(slq, _connection);
            var affectedRowsCount = cmd.ExecuteNonQuery().ToString();
            Console.WriteLine($"Created STUDENTS table. Affected rows count: {affectedRowsCount}");
        }
        public void CreateLessonSubjectsTables() {

            var sql = @"
                CREATE SEQUENCE IF NOT EXISTS lessonSubjects_id_seq;

                CREATE TABLE IF NOT EXISTS lessonSubjects (
                    id              BIGINT                  NOT NULL        DEFAULT NEXTVAL('lessonSubjects_id_seq'),
                    student_id      BIGINT                  NOT NULL,
                    tutor_id        BIGINT                  NOT NULL,
                    tytle           CHARACTER VARYING(255)  NOT NULL,
                    place           CHARACTER VARYING(255)  NOT NULL,
                    result          CHARACTER VARYING(255)  NOT NULL,
                    notes           CHARACTER VARYING(255)  NOT NULL,
                    points          INTEGER                 NOT NULL,
                    
                    CONSTRAINT  lessonSubjects_pkey PRIMARY  KEY (id),
                    CONSTRAINT  lessonSubjects_fk_student_id FOREIGN KEY (student_id) REFERENCES students(id) ON DELETE SET NULL,
                    CONSTRAINT  lessonSubjects_fk_tutor_id FOREIGN KEY (tutor_id) REFERENCES tutors(id) ON DELETE SET NULL
                    
                );
            ";
            using var cmd = new NpgsqlCommand(sql, _connection);
            var affectedRowsCount = cmd.ExecuteNonQuery().ToString();
            Console.WriteLine($"Created STUDENTS table. Affected rows count: {affectedRowsCount}");
        }
        public long InsertDataForStuents(StudentModel student) {

            var sql = @"
                INSERT INTO students(first_name, last_name, middle_name, email, age)
                VALUES(:first_name, :middle_name, :last_name, :email, :age)
                RETURNING id
                ";
            using var cmd = new NpgsqlCommand( sql, _connection);
            var parameters = cmd.Parameters;
            parameters.Add(new NpgsqlParameter("first_name", student.FirstName));
            parameters.Add(new NpgsqlParameter("middle_name", student.MiddleName));
            parameters.Add(new NpgsqlParameter("last_name", student.LastName));
            parameters.Add(new NpgsqlParameter("email", student.Email));
            parameters.Add(new NpgsqlParameter("age", student.Age));
            var resId = (long)cmd.ExecuteScalar();
            Console.WriteLine($"Inserted to STUDENTS table. id: {resId}");
            return resId;
        }
        public long InsertDataForTutors(TutorModel tutor) {

            var sql = @"
                INSERT INTO tutors(first_name, last_name, middle_name, email, age)
                VALUES(:first_name, :middle_name, :last_name, :email, :age)
                RETURNING id
                ";
            using var cmd = new NpgsqlCommand(sql, _connection);
            var parameters = cmd.Parameters;
            parameters.Add(new NpgsqlParameter("first_name", tutor.FirstName));
            parameters.Add(new NpgsqlParameter("middle_name", tutor.MiddleName));
            parameters.Add(new NpgsqlParameter("last_name", tutor.LastName));
            parameters.Add(new NpgsqlParameter("email", tutor.Email));
            parameters.Add(new NpgsqlParameter("age", tutor.Age));
            var resId = (long)cmd.ExecuteScalar();
            Console.WriteLine($"Inserted for TUTORS table. id: {resId}");
            return resId;
        }
        public long InsertDataForLessonSubjects(LessonSubjectModel lessonSubject) {

            var sql = @"
                INSERT INTO lessonSubjects(student_Id, tutor_Id, tytle, place, result, notes, points)
                VALUES(:student_Id, :tutor_Id,:tytle, :place, :result, :notes, :points)
                RETURNING id
                ";
            using var cmd = new NpgsqlCommand(sql, _connection);
            var parameters = cmd.Parameters;
            parameters.Add(new NpgsqlParameter("student_Id", lessonSubject.StudentId));
            parameters.Add(new NpgsqlParameter("tutor_Id", lessonSubject.TutorId));
            parameters.Add(new NpgsqlParameter("tytle", lessonSubject.Tytle));
            parameters.Add(new NpgsqlParameter("place", lessonSubject.Place));
            parameters.Add(new NpgsqlParameter("result", lessonSubject.Result));
            parameters.Add(new NpgsqlParameter("notes", lessonSubject.Notes));
            parameters.Add(new NpgsqlParameter("points", lessonSubject.Points));
            var resId = (long)cmd.ExecuteScalar();
            Console.WriteLine($"Inserted for LESSONSUBJECTS table. id: {resId}");
            return resId;
        }
        public void ReadData() {
            var sql = @"
                SELECT * FROM students
                ORDER BY id ASC
            ";
            using (var cmd = new NpgsqlCommand(sql, _connection)) {
                
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Console.WriteLine();
                    var id = reader.GetInt64(0);
                    var fn = reader.GetString(1);
                    var mn = reader.GetString(2);
                    var ln = reader.GetString(3);
                    var email = reader.GetString(4);
                    var age = reader.GetInt32(5);
                    Console.Write($"student id: {id}");
                    Console.Write($"[first name = {fn}]");
                    Console.Write($"[middle name = {mn}]");
                    Console.Write($"[last name = {ln}]");
                    Console.Write($"[email name = {email}]");
                    Console.Write($"[age name = {age}]");

                }
            }
            Console.WriteLine();
            Console.WriteLine(new String('*', 150));
            sql = @"
                SELECT * FROM tutors
                ORDER BY id ASC
            ";
            using (var cmd = new NpgsqlCommand(sql, _connection)) {
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Console.WriteLine();
                    var id = reader.GetInt64(0);
                    var fn = reader.GetString(1);
                    var mn = reader.GetString(2);
                    var ln = reader.GetString(3);
                    var email = reader.GetString(4);
                    var age = reader.GetInt32(5);
                    Console.Write($"tutor id: {id}");
                    Console.Write($"[first name = {fn}]");
                    Console.Write($"[middle name = {mn}]");
                    Console.Write($"[last name = {ln}]");
                    Console.Write($"[email name = {email}]");
                    Console.Write($"[age name = {age}]");

                }
            }
            Console.WriteLine();
            Console.WriteLine(new String('*', 150));
            sql = @"
                SELECT * FROM lessonSubjects
                ORDER BY id ASC
            ";
            using (var cmd = new NpgsqlCommand(sql, _connection)) {
                using var reader = cmd.ExecuteReader();
                while (reader.Read()) {
                    Console.WriteLine();
                    var id = reader.GetInt64(0);
                    var student_Id = reader.GetInt64(1);
                    var tutor_Id = reader.GetInt64(2);
                    var tytle = reader.GetString(3);
                    var place = reader.GetString(4);
                    var result = reader.GetString(5);
                    var notes = reader.GetString(6);
                    var points = reader.GetInt32(7);
                    Console.Write($"lessonSubject id: {id}");
                    Console.Write($"[student_Id = {student_Id}]");
                    Console.Write($"[tutor_Id = {tutor_Id}]");
                    Console.Write($"[tytle = {tytle}]");
                    Console.Write($"[place = {place}]");
                    Console.Write($"[result = {result}]");
                    Console.Write($"[notes = {notes}]");
                    Console.Write($"[points = {points}]");

                }
                Console.WriteLine();
            }
        }
        public void InsertFiveRowsIntoTables() {
            //1
            long studentId = InsertDataForStuents(new StudentModel() {
                FirstName = "Ivan",
                MiddleName = "Ivanov",
                LastName = "Ivanovich",
                Age = 23,
                Email = "if7van@gmail.com"
            });
            long tutorId = InsertDataForTutors(new TutorModel() {
                FirstName = "Vania",
                MiddleName = "Vankin",
                LastName = "Vankovich",
                Age = 45,
                Email = "va6nia@gmail.com"
            });
            InsertDataForLessonSubjects(new LessonSubjectModel() {
                TutorId = tutorId,
                StudentId = studentId,
                Tytle = "SupperTytle",
                Place = "Room21",
                Points = 3,
                Result = "Exelent",
                Notes = "none"
            });

            //2
            studentId = InsertDataForStuents(new StudentModel() {
                FirstName = "Ivan",
                MiddleName = "Ivanov",
                LastName = "Ivanovich",
                Age = 23,
                Email = "ifv5an@gmail.com"
            });
            tutorId = InsertDataForTutors(new TutorModel() {
                FirstName = "Vania",
                MiddleName = "Vankin",
                LastName = "Vankovich",
                Age = 45,
                Email = "van4ia@gmail.com"
            });
            InsertDataForLessonSubjects(new LessonSubjectModel() {
                TutorId = tutorId,
                StudentId = studentId,
                Tytle = "SupperTytle",
                Place = "Room21",
                Points = 3,
                Result = "Exelent",
                Notes = "none"
            });

            //3
            studentId = InsertDataForStuents(new StudentModel() {
                FirstName = "Ivan",
                MiddleName = "Ivanov",
                LastName = "Ivanovich",
                Age = 23,
                Email = "if3van@gmail.com"
            });
            tutorId = InsertDataForTutors(new TutorModel() {
                FirstName = "Vania",
                MiddleName = "Vankin",
                LastName = "Vankovich",
                Age = 45,
                Email = "van2ia@gmail.com"
            });
            InsertDataForLessonSubjects(new LessonSubjectModel() {
                TutorId = tutorId,
                StudentId = studentId,
                Tytle = "SupperTytle",
                Place = "Room21",
                Points = 3,
                Result = "Exelent",
                Notes = "none"
            });


            //4
            studentId = InsertDataForStuents(new StudentModel() {
                FirstName = "Ivan",
                MiddleName = "Ivanov",
                LastName = "Ivanovich",
                Age = 23,
                Email = "ifvan@g1mail.com"
            });
            tutorId = InsertDataForTutors(new TutorModel() {
                FirstName = "Vania",
                MiddleName = "Vankin",
                LastName = "Vankovich",
                Age = 45,
                Email = "vanaia@gmail.com"
            });
            InsertDataForLessonSubjects(new LessonSubjectModel() {
                TutorId = tutorId,
                StudentId = studentId,
                Tytle = "SupperTytle",
                Place = "Room21",
                Points = 3,
                Result = "Exelent",
                Notes = "none"
            });


            //5
            studentId = InsertDataForStuents(new StudentModel() {
                FirstName = "Ivan",
                MiddleName = "Ivanov",
                LastName = "Ivanovich",
                Age = 23,
                Email = "ifvasn@gmail.com"
            });
            tutorId = InsertDataForTutors(new TutorModel() {
                FirstName = "Vania",
                MiddleName = "Vankin",
                LastName = "Vankovich",
                Age = 45,
                Email = "vanfia@gmail.com"
            });
            InsertDataForLessonSubjects(new LessonSubjectModel() {
                TutorId = tutorId,
                StudentId = studentId,
                Tytle = "SupperTytle",
                Place = "Room21",
                Points = 3,
                Result = "Exelent",
                Notes = "none"
            });
        }
    }
}
