using System;

namespace SchoolTemplate.Database
{
  public class FestivalDag
  {
    public int Id { get; set; }
    
    public int festivalId { get; set; }

    public DateTime start { get; set; }

        public String festival_naam { get; set; }

        public DateTime end { get; set; }    

    /// <summary>
    /// Gebruik altijd decimal voor geldzaken. Dit doe je om te voorkomen dat er afrondingsfouten optreden
    /// </summary>

    public string voorraad { get; set; }

  }
}
