using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.IO;
using System.Linq;

namespace eBayPulse.Models
{
    public partial class Item
    {
        public int Id {get; set;}
        public string eBayId {get; set;}
        public string Name {get; set;}
        public long UpdatePeriod_Sec {get; set;}

        //[InverseProperty("Item")]
        public virtual ICollection<Pulse> Pulses {get; set;}

        //[InverseProperty("Item")]
        public ICollection<Note> Notes {get; set;}

        public Item() 
        { 
            Pulses = new List<Pulse>();//eBayPulse.Models.DBConnector.getConnection().context.Pulse.Where(x => x.ItemId == Id).ToList();
            Notes = new List<Note>();
        }
    }
    public partial class Pulse 
    {
        public int Id {get; set;}
        [ForeignKey("Item")]
        public int? ItemId {get; set;}
        public long Unix_Time {get; set;}
        public int Views {get; set;}
        public int Watchers {get; set;}
        
        //[InverseProperty("Pulses")]
        public Item Item {get; set;}

        public Pulse() { }
    }

    public class Note
    {
        public int Id {get; set;}
        [ForeignKey("Item")]
        public int? ItemId {get; set;}
        public long Unix_Time {get; set;}
        
        public string Text {get; set;}
        
        //[InverseProperty("Notes")]
        public Item Item {get; set;}
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Pulse>()
            .HasOne(g => g.Item)
            .WithMany(e => e.Pulses)
            .HasForeignKey(c => c.ItemId); 
        }
    }
}