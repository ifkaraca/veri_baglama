using Kullanici.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Kullanici.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProcController : ControllerBase
    {
        private readonly string connectionString; 

        public ProcController(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:SqlServerDB"] ?? ""; 
        }

        [HttpGet]
        public IActionResult GetTumTablo()
        {
            List<LiKullanici> Kullanicis = new List<LiKullanici>();
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "exec TumGetir";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        using(var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LiKullanici kullanici=new LiKullanici();

                                kullanici.id=reader.GetInt32(0);
                                kullanici.ad=reader.GetString(1);
                                kullanici.soyad=reader.GetString(2);
                                kullanici.telefon=reader.GetString(3);
                                kullanici.sehir=reader.GetString(4);

                                Kullanicis.Add(kullanici);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }
            return Ok(Kullanicis);
        }

        [HttpPost]
        public IActionResult InsertKulanici(KullanciDto kullanciDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "exec InsertKullanici @ad,@soyad,@telefon,@sehir";

                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ad", kullanciDto.ad);
                        command.Parameters.AddWithValue("@soyad", kullanciDto.soyad);
                        command.Parameters.AddWithValue("@telefon", kullanciDto.telefon);
                        command.Parameters.AddWithValue("@sehir", kullanciDto.sehir);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetirId(int id)
        {
            LiKullanici kullanici = new LiKullanici();
            try
            {
                using(var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "exec GetirId @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kullanici.id = reader.GetInt32(0);
                                kullanici.ad = reader.GetString(1);
                                kullanici.soyad = reader.GetString(2);
                                kullanici.telefon = reader.GetString(3);
                                kullanici.sehir = reader.GetString(4);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }
            return Ok(kullanici);
        }


        [HttpPut("{id}")]
        public IActionResult DuzenleTum(int id,KullanciDto kullanciDto)
        {
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string sql = "exec DuzenleTum @id,@ad,@soyad,@telefon,@sehir";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@ad", kullanciDto.ad);
                        command.Parameters.AddWithValue("@soyad", kullanciDto.soyad);
                        command.Parameters.AddWithValue("@telefon", kullanciDto.telefon);
                        command.Parameters.AddWithValue("@sehir", kullanciDto.sehir);
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }
            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult TumSil(int id)
        {
            try
            {
                using (var conncetion = new SqlConnection(connectionString))
                {
                    conncetion.Open();
                    string sql = "exec TumSil @id";
                    using (var command = new SqlCommand(sql, conncetion))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }
            return Ok();
        }



        [HttpGet("telefon/{id}")]
        public IActionResult GetTelefon(int id)
        {
            TlfProc kullanici = new TlfProc();
            try
            {
                using (var connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    string sql = "exec telefongetir @id";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                kullanici.ad = reader.GetString(0);
                                kullanici.telefon = reader.GetString(1);
                            }
                            else
                            {
                                return NotFound();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu: " + ex.Message); // Hata mesajını güncelledik
                return BadRequest(ModelState);
            }
            return Ok(kullanici);
        }
    }
}
