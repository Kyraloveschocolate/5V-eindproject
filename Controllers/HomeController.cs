﻿using System;
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


        [Route("festival/{id}")]
        public IActionResult Festival (string id)
        {
            var model = GetFestival(id);
            return View(model);
        }

        private Festival GetFestival (string id)
        { 
            
            List<Festival> festivals= new List<Festival>();

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
            return View();
        }
        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
      return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
  }
}
