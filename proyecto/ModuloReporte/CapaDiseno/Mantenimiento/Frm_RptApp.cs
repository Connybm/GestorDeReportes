﻿using System;
using System.Windows.Forms;
using capaDato.Entity;
using CapaControl.Control;
using CapaDiseno.Dialogos;
using System.Collections.Generic;

namespace CapaDiseno.Mantenimiento
{
    public partial class Frm_RptApp : Form
    {
        private ReporteAplicacionControl reporteAppControl = new ReporteAplicacionControl();
        private ReporteAplicacion reporteApp;
        private string accion;

        public Frm_RptApp()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            llenarDgv();
            iniciazliarTbpConsulta();
        }

        private void llenarDgv()
        {
            int fila = 0;
            Dgv_Consulta.Rows.Clear();
            foreach (ReporteAplicacion reporteAppTmp in reporteAppControl.obtenerAllReporteApp())
            {
                Dgv_Consulta.Rows.Add();
                Dgv_Consulta.Rows[fila].Cells[0].Value = reporteAppTmp.REPORTE.NOMBRE;
                Dgv_Consulta.Rows[fila].Cells[1].Value = reporteAppTmp.APLICACION.NOMBRE;
                Dgv_Consulta.Rows[fila].Cells[2].Value = reporteAppTmp.MODULO.NOMBRE;
                Dgv_Consulta.Rows[fila].Cells[3].Value = reporteAppTmp.ESTADO.ToString();
                fila++;
            }
        }

        public void llenarCmbReporte()
        {
            ReporteControl reporteControl = new ReporteControl();
            List<Reporte> reporteList = reporteControl.obtenerAllReporte(); 

            Cmb_Reporte.ValueMember = "REPORTE";
            Cmb_Reporte.DisplayMember = "NOMBRE";
            Cmb_Reporte.DataSource = reporteList;
        }

        public void llenarCmbModulo()
        {
            ModuloControl moduloControl = new ModuloControl();
            List<Modulo> moduloList = moduloControl.obtenerAllModulo();

            Cmb_Modulo.ValueMember = "MODULO";
            Cmb_Modulo.DisplayMember = "NOMBRE";
            Cmb_Modulo.DataSource = moduloList;
        }
        public void llenarCmbAplicacion()
        {
            Modulo mdlTmp = (Modulo)Cmb_Modulo.SelectedItem;
            AplicacionControl aplicacionControl = new AplicacionControl();
            List<Aplicacion> AplicacionList = aplicacionControl.obtenerAllAplicacionByMdl(mdlTmp.MODULO);

            Cmb_Aplicacion.ValueMember = "APLICACION";
            Cmb_Aplicacion.DisplayMember = "NOMBRE";
            Cmb_Aplicacion.DataSource = AplicacionList;
        }

        private void habilitarCampos()
        {
            Cmb_Reporte.Enabled = true;
            Cmb_Modulo.Enabled = true;
            Cmb_Aplicacion.Enabled = true;
            Txt_Estado.Enabled = true;
        }

        private void deshabilitarCampos()
        {
            Cmb_Reporte.Enabled = false;
            Cmb_Modulo.Enabled = false;
            Cmb_Aplicacion.Enabled = false;
            Txt_Estado.Enabled = false;
        }

        private void iniciazliarTbpConsulta()
        {
            habilitarCampos();
            llenarCmbReporte();
            llenarCmbModulo();
            llenarCmbAplicacion();
            Txt_Estado.Text = "1";
        }

        private ReporteAplicacion llenarReporteApp()
        {
            ReporteAplicacion reporteAppTmp = new ReporteAplicacion();
            reporteAppTmp.REPORTE = (Reporte)Cmb_Reporte.SelectedItem;
            reporteAppTmp.MODULO = (Modulo)Cmb_Modulo.SelectedItem;
            reporteAppTmp.APLICACION = (Aplicacion)Cmb_Aplicacion.SelectedItem;
            reporteAppTmp.ESTADO = int.Parse(Txt_Estado.Text);

            return reporteAppTmp;
        }

        private void llenarTbpDato(ReporteAplicacion reporteApp)
        {
            deshabilitarCampos();
            Cmb_Reporte.SelectedItem = Cmb_Reporte.Items[reporteApp.REPORTE.REPORTE];
            Cmb_Modulo.SelectedItem = Cmb_Modulo.Items[reporteApp.MODULO.MODULO];
            Cmb_Aplicacion.SelectedItem = Cmb_Aplicacion.Items[reporteApp.APLICACION.APLICACION];
            Txt_Estado.Text = reporteApp.ESTADO.ToString();
        }

        /*
         * Programacion botones.
         */

        private void Btn_Guardar_Click(object sender, EventArgs e)
        {
            this.reporteApp = llenarReporteApp();

            Dialogo dialogo = new Dialogo();
            bool confirmacion = dialogo.dialogoSiNo("Confirmacion", "Desea guardar?");
            if (confirmacion)
            {
                if (this.accion == "nuevo")
                {
                    reporteAppControl.insertarReporteApp(this.reporteApp);
                }
                else if (this.accion == "modificar")
                {
                    reporteAppControl.actualizarReporteApp(this.reporteApp);
                }
            }
        }

        private void Cmb_Modulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            llenarCmbAplicacion();
        }

        private void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            iniciazliarTbpConsulta();
            this.reporteApp = new ReporteAplicacion();
            Tbc_RptApp.SelectedTab = Tbp_Datos;
            this.accion = "nuevo";
        }

        private void Btn_Modificar_Click(object sender, EventArgs e)
        {
            habilitarCampos();
            this.reporteApp = llenarReporteApp();
            this.accion = "modificar";
        }

        private void Btn_Borrar_Click(object sender, EventArgs e)
        {
            this.accion = null;
            Dialogo dialogo = new Dialogo();
            bool confirmacion = dialogo.dialogoSiNo("Confirmacion", "Desea eliminar?");

            if (confirmacion)
            {
                reporteAppControl.eliminarReporteApp(this.reporteApp.APLICACION.APLICACION, this.reporteApp.MODULO.MODULO);
                this.reporteApp = new ReporteAplicacion();

                iniciazliarTbpConsulta();
                Tbc_RptApp.SelectedTab = Tbp_Consulta;
                llenarDgv();
            }
        }

        private void Btn_Cancelar_Click(object sender, EventArgs e)
        {
            iniciazliarTbpConsulta();
            Tbc_RptApp.SelectedTab = Tbp_Consulta;
            this.reporteApp = new ReporteAplicacion();
            llenarDgv();
        }

        private void seleccionarRegistro(object sender, DataGridViewCellEventArgs e)
        {
            int fila = Dgv_Consulta.CurrentCell.RowIndex;
            String codigoApp = Dgv_Consulta.Rows[fila].Cells[1].Value.ToString();
            String codigoMdl = Dgv_Consulta.Rows[fila].Cells[2].Value.ToString();
            this.reporteApp = reporteAppControl.obtenerReporteApp(int.Parse(codigoApp), Int32.Parse(codigoMdl));
            llenarTbpDato(this.reporteApp);
            Tbc_RptApp.SelectedTab = Tbp_Datos;
        }
    }
}
