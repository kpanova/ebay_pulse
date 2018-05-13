using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.IO;
namespace eBayPulse
{
    public partial class Item : IDataReciver
    {
        public int Id {get; set;}
        public string eBayId {get; set;}
        public string Name {get; set;}
        public int UpdatePeriod {get; set;}

        [InverseProperty("Item")]
        public virtual ICollection<Pulse> Pulses {get; set;}

        [InverseProperty("Item")]
        public virtual ICollection<Note> Notes {get; set;}

        public Item() { }
    }
    public class Pulse 
    {
        public int Id {get; set;}
        public int? ItemId {get; set;}
        public int Time {get; set;}
        public int Views {get; set;}
        public int Watchers {get; set;}

        [InverseProperty("Pulses")]
        public virtual Item Item {get; set;}

        public Pulse() { }
    }

    public class Note
    {
        public int Id {get; set;}
        public int? ItemId {get; set;}
        public int Time {get; set;}
        
        public string Text {get; set;}

        [InverseProperty("Notes")]
        public virtual Item Item {get; set;}
        public Note() { }

    }    
    public class eBayPulseContext : DbContext
    {
        public DbSet<Item> Item { get; set; }
        public DbSet<Pulse> Pulse { get; set; }
        public DbSet<Note> Note { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var directoryPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "eBayPulse"
            );

            // Если папка не существует до создать
            if(!Directory.Exists(directoryPath)){
                Directory.CreateDirectory(directoryPath);
            }

            var databasePath = Path.Combine(
                directoryPath,
                "eBayPulse.sqlite"
            );
            
            optionsBuilder.UseSqlite("filename=" + databasePath);
        }
    }
}