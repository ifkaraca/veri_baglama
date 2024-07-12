using Kullanici.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace Kullanici.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KullaniciController : ControllerBase
    {
        private readonly string conncetionString;

        public KullaniciController(IConfiguration configuration)
        {
            conncetionString = configuration["ConnectionStrings:SqlServerDB"]??"";
        }

        [HttpPost]
        public IActionResult CreateKullanici(KullanciDto kullanciDto)
        {

            try
            {
                using (var connection = new SqlConnection(conncetionString))
                {
                    connection.Open();
                    string sql = "insert into kullanici_tbl" +
                        "(kullanici_adi,kullanici_soyad,kullanici_tlf,kullanici_sehir) values" +
                        "(@ad,@soyad,@telefon,@sehir)";

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
            catch (Exception ex) 
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }

            return Ok();
        }

        [HttpGet]
        public IActionResult GetKullanici()
        {
            
            List<LiKullanici> kullanicis = new List<LiKullanici>();

            try
            {
                using (var conncetion = new SqlConnection(conncetionString))
                {
                    conncetion.Open();
                    string sql = "select * from kullanici_tbl";
                    using (var command = new SqlCommand(sql, conncetion))
                    {
                        using(var reader =command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LiKullanici kullanici = new LiKullanici();

                                kullanici.id = reader.GetInt32(0);
                                kullanici.ad=reader.GetString(1);
                                kullanici.soyad=reader.GetString(2);
                                kullanici.telefon=reader.GetString(3);
                                kullanici.sehir=reader.GetString(4);

                                kullanicis.Add(kullanici);
                            }
                        }
                    }
                }
            }
            catch(Exception ex) 
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }

            return Ok(kullanicis);
        }

        [HttpGet("{id}")]
        public IActionResult GetKullaniciid( int id )
        {
            LiKullanici kullanici = new LiKullanici();
            try
            {
                using (var conncetion = new SqlConnection(conncetionString))
                {
                    conncetion.Open();
                    string sql = "select * from kullanici_tbl where id=@id";
                    using (var command = new SqlCommand(sql, conncetion))
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
        public IActionResult UpdateKullanici(int id, KullanciDto kullaniciDto)
        {
            try
            {
                using (var conncetion = new SqlConnection(conncetionString))
                {
                    conncetion.Open();

                    string sql = "update kullanici_tbl set kullanici_adi=@ad, kullanici_soyad=@soyad," +
                        "kullanici_tlf=@telefon, kullanici_sehir=@sehir where id=@id";
                    using (var command = new SqlCommand(sql, conncetion))
                    {
                        command.Parameters.AddWithValue("@ad",kullaniciDto.ad);
                        command.Parameters.AddWithValue("@soyad", kullaniciDto.soyad);
                        command.Parameters.AddWithValue("@telefon", kullaniciDto.telefon);
                        command.Parameters.AddWithValue("@sehir", kullaniciDto.sehir);
                        command.Parameters.AddWithValue("@id",id);

                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Kullanıcı", "Üzgünüz ama hata oldu");
                return BadRequest(ModelState);
            }
            return Ok();//kullaniciDto
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteKullanici(int id)
        {
            try
            {
                using (var conncetion = new SqlConnection(conncetionString))
                {
                    conncetion.Open();
                    string sql = "delete from kullanici_tbl where id=@id";
                    using (var command = new SqlCommand(sql,conncetion))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        command.ExecuteNonQuery ();
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
    }
}
