using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using SchoolTemplate.Database;
using SchoolTemplate.Models;

namespace SchoolTemplate.Controllers
{
    public class HomeController : Controller
    {
        // zorg ervoor dat je hier je gebruikersnaam (leerlingnummer) en wachtwoord invult
        string connectionString = "Server=172.16.160.21;Port=3306;Database=110289;Uid=110289;Pwd=orUceAKi;";

        public IActionResult Index()
        {
            List<Festival> Festival = new List<Festival>();
            // uncomment deze regel om producten uit je database toe te voegen
            Festival = GetFestivals();

            return View(Festival);
        }


        private List<Festival> GetFestivals()
        {
            List<Festival> festivals = new List<Festival>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("select * from Festival", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Festival F = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Datum = DateTime.Parse(reader["Datum"].ToString()),
                            Prijs = Decimal.Parse(reader["Prijs"].ToString())
                        };
                        festivals.Add(F);
                    }
                }
            }

            return festivals;

        }

        private Festival GetFestival(string id)
        {
            List<Festival> festivals = new List<Festival>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from festival where id = {id}", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Festival p = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Datum = DateTime.Parse(reader["Datum"].ToString()),
                            Prijs = Decimal.Parse(reader["Prijs"].ToString())
                        };
                        festivals.Add(p);
                    }
                }
            }

            return festivals[0];

        }

        private List<FestivalDag> GetFestivalDags()
        {
            List<FestivalDag> festivaldags = new List<FestivalDag>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from festival_dag", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        FestivalDag D = new FestivalDag
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            festivalId = Convert.ToInt32(reader["festival_id"]),
                            festival_naam = (reader["festival_naam"].ToString()),
                            start = DateTime.Parse(reader["start"].ToString()),
                            end = DateTime.Parse(reader["end"].ToString()),
                            voorraad = reader["voorraad"].ToString(),

                        };
                    festivaldags.Add(D);
                    }
                }
            }

            return festivaldags;

        }
    
      

        private void SavePerson(PersonModel person)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand("INSERT INTO klant(naam, achternaam, emailadres) VALUES(?voornaam, ?achternaam, ?email)", conn);

                cmd.Parameters.Add("?voornaam", MySqlDbType.VarChar).Value = person.Voornaam;
                cmd.Parameters.Add("?achternaam", MySqlDbType.VarChar).Value = person.Achternaam;
                cmd.Parameters.Add("?email", MySqlDbType.VarChar).Value = person.Email;
                cmd.ExecuteNonQuery();
            }
        }

        private Festival GetFestivalDag(string id)
        {

            List<Festival> festivals = new List<Festival>();

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand($"select * from Festival where id = {id}", conn);

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Festival F = new Festival
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Naam = reader["Naam"].ToString(),
                            Beschrijving = reader["Beschrijving"].ToString(),
                            Datum = DateTime.Parse(reader["Datum"].ToString()),
                            Prijs = Decimal.Parse(reader["Prijs"].ToString())
                        };
                        festivals.Add(F);
                    }
                }
            }

            return festivals[0];
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [Route("agenda")]
        public IActionResult Agenda()
        {
            return View();
        }
        [Route("tickets")]
        public IActionResult Tickets()
        {
            var festivalDagen = GetFestivalDags();
            return View(festivalDagen);
        }

        [Route("festival/{id}")]
        public IActionResult Festival(string id)
        {
            var model = GetFestival(id);
            return View(model);
        }
       


        [Route("contact")]
     
        public IActionResult Contact (string voornaam, string achternaam)
        {
            ViewData["voornaam"] = voornaam;
            ViewData["achternaam"] = achternaam;
            return View();
        }

        [Route("contact")]
        [HttpPost]
        public IActionResult Contact(PersonModel model)
        {
           // geen valide model? Dan tonen we de foutmeldingen
           if(!ModelState.IsValid)
            return View(model);

            // Model is valide. Dus we kunnen opslaan
            SavePerson(model);

            ViewData["formsucces"] = "ök";

            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("show-all")]
        public IActionResult ShowAll()
        {
            return View();
        }

    }
}
