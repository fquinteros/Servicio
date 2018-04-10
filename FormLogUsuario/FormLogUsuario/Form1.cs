using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//hacemos uso de oracle
using System.Data.OracleClient;

namespace FormLogUsuario
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Establecemos la conexion con la base de datos
        OracleConnection ora = new OracleConnection("DATA SOURCE = orcl; USER ID= prueba; PASSWORD=123;");
        private void btnProbarConexion_Click(object sender, EventArgs e)
        {
            //Este boton permite comprobar si se puede establecer la conexion con la DB
            try
            {
                ora.Open();
                MessageBox.Show("Conectado");
                ora.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al conectar");               
            }

        }

        private void btnCargar_Click(object sender, EventArgs e)
        {
            //Alternativa 1 haciendo procedimiento en el metodo

            /*ora.Open();
            string sql = "select * from usuario";
            OracleCommand cmd = new OracleCommand(sql, ora);
            cmd.CommandType = System.Data.CommandType.Text;

            OracleDataAdapter adaptador = new OracleDataAdapter();
            adaptador.SelectCommand = cmd;
            DataTable dsRetorno = new DataTable();
            adaptador.Fill(dsRetorno);
            dgvUsuario.DataSource = dsRetorno;

            ora.Close();*/

            //Alternativa 2 haciendo el procedimiento en la DB

            try
            {
                ora.Open();
                OracleCommand cmd = new OracleCommand("selecionarusuario", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("registros", OracleType.Cursor).Direction = ParameterDirection.Output;

                OracleDataAdapter adaptador = new OracleDataAdapter();
                adaptador.SelectCommand = cmd;
                DataTable tabla = new DataTable();
                adaptador.Fill(tabla);
                dgvUsuario.DataSource = tabla;

                ora.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("Error al cargar los datos");
            }

        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            try
            {
                ora.Open();
                OracleCommand cmd = new OracleCommand("crear", ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("rutX", OracleType.VarChar).Value = txtRut.Text;
                cmd.Parameters.Add("dvX", OracleType.VarChar).Value = txtDv.Text;
                cmd.Parameters.Add("cont", OracleType.VarChar).Value = txtContrasenia.Text;
                cmd.Parameters.Add("tip", OracleType.VarChar).Value = txtTipo.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario creado");
                ora.Close();
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo crear el usuario");
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            try
            {
                ora.Open();
                OracleCommand cmd = new OracleCommand("actualizar",ora);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("rutU", OracleType.VarChar).Value = txtRutIdentificar.Text;
                cmd.Parameters.Add("dvU",OracleType.VarChar).Value = txtDv.Text;
                cmd.Parameters.Add("contU", OracleType.VarChar).Value = txtContrasenia.Text;
                cmd.Parameters.Add("tipU",OracleType.VarChar).Value = txtTipo.Text;
                cmd.ExecuteNonQuery();
                MessageBox.Show("Usuario actualizado");
                ora.Close();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            ora.Open();
            OracleCommand cmd = new OracleCommand("eliminar", ora);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Add("rutU",OracleType.VarChar).Value = txtRutEliminar.Text;
            cmd.Parameters.Add("dvU", OracleType.VarChar).Value = txtDvEliminar.Text;
            cmd.ExecuteNonQuery();
            MessageBox.Show("Usuario eliminado");
            ora.Close();
        }
    }
}
