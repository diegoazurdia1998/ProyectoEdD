using System;

namespace Salud_para_tu_sonrisa.Models
{
    public class Paciente
    {
        public String Name { get; set; }
        public String ID { get; set; }
        public int Age { get; set; }
        public String Phone { get; set; }
        public DateTime LastDate { get; set; }
        public DateTime NextDate { get; set; }
        public String Description { get; set; }
        public Paciente(String name, String id, int age, String phone, DateTime last)
        {
            this.Name = name;
            this.ID = id;
            this.Age = age;
            this.Phone = phone;
            this.LastDate = last;
        }
        public Paciente(String name, String id, int age, String phone, DateTime last, DateTime next)
        {
            this.Name = name;
            this.ID = id;
            this.Age = age;
            this.Phone = phone;
            this.LastDate = last;
            this.NextDate = next;
        }
        public Paciente(String name, String id, int age, String phone, DateTime last, String desc)
        {
            this.Name = name;
            this.ID = id;
            this.Age = age;
            this.Phone = phone;
            this.LastDate = last;
            this.Description = desc;
        }
        public Paciente(String name, String id, int age, String phone, DateTime last, DateTime next, String desc)
        {
            this.Name = name;
            this.ID = id;
            this.Age = age;
            this.Phone = phone;
            this.LastDate = last;
            this.NextDate = next;
            this.Description = desc;
        }
    }
}
