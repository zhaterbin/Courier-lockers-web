﻿using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Courier_lockers.Entities
{
    [Table("User")]
    public class User
    {
        [Column("Id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Column("FirstName")]
        public string? FirstName { get; set; }
        [Column("LastName")]
        public string? LastName { get; set; }
        [Column("Username")]
        public string Username { get; set; }
        [Column("Password")]
        [JsonIgnore]
        public string Password { get; set; }
        //[column]
        //public IEnumerable<string> Roles { get; set; }
    }
}
