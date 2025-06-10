using Microsoft.AspNetCore.Mvc;
using UsuarioApi.Models;
using System.Data.SqlClient;

namespace UsuarioApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetUsuarios()
        {
            try
            {
                List<Usuario> lista = new();
                using var con = new SqlConnection(_configuration.GetConnectionString("ConexionDB"));
                using var cmd = new SqlCommand("EXEC sp_Usuarios @Accion='CONSULTAR'", con);
                con.Open();
                using var dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    lista.Add(new Usuario
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Sexo = dr["Sexo"].ToString()
                    });
                }
                return Ok(new { success = true, data = lista });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al consultar los usuarios.", detail = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult AgregarUsuario([FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest(new { success = false, message = "El usuario no puede ser nulo." });

            try
            {
                using var con = new SqlConnection(_configuration.GetConnectionString("ConexionDB"));
                using var cmd = new SqlCommand("sp_Usuarios", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Accion", "AGREGAR");
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                con.Open();
                cmd.ExecuteNonQuery();
                return Created("", new { success = true, message = "Usuario creado exitosamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al crear el usuario.", detail = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public IActionResult ModificarUsuario(int id, [FromBody] Usuario usuario)
        {
            if (usuario == null)
                return BadRequest(new { success = false, message = "El usuario no puede ser nulo." });

            try
            {
                using var con = new SqlConnection(_configuration.GetConnectionString("ConexionDB"));
                using var cmd = new SqlCommand("sp_Usuarios", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Accion", "MODIFICAR");
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Sexo", usuario.Sexo);
                con.Open();
                cmd.ExecuteNonQuery();
                return Ok(new { success = true, message = "Usuario modificado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al modificar el usuario.", detail = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult EliminarUsuario(int id)
        {
            try
            {
                using var con = new SqlConnection(_configuration.GetConnectionString("ConexionDB"));
                using var cmd = new SqlCommand("sp_Usuarios", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Accion", "ELIMINAR");
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
                return Ok(new { success = true, message = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al eliminar el usuario.", detail = ex.Message });
            }
        }



        [HttpGet("{id}")]
        public IActionResult GetUsuarioPorId(int id)
        {
            try
            {
                Usuario? usuario = null;
                using var con = new SqlConnection(_configuration.GetConnectionString("ConexionDB"));
                using var cmd = new SqlCommand("sp_Usuarios", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Accion", "CONSULTAR_POR_ID");
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                using var dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    usuario = new Usuario
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Nombre = dr["Nombre"].ToString(),
                        FechaNacimiento = Convert.ToDateTime(dr["FechaNacimiento"]),
                        Sexo = dr["Sexo"].ToString()
                    };
                }

                if (usuario == null)
                {
                    return NotFound(new { success = false, message = "Usuario no encontrado." });
                }

                return Ok(new { success = true, data = usuario });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Error al obtener el usuario.", detail = ex.Message });
            }
        }

    }
}
