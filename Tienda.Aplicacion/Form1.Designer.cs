namespace Tienda.Aplicacion
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.Venta = new System.Windows.Forms.Button();
            this.Entrada = new System.Windows.Forms.Button();
            this.Deuda = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Venta
            // 
            this.Venta.Location = new System.Drawing.Point(523, 160);
            this.Venta.Name = "Venta";
            this.Venta.Size = new System.Drawing.Size(159, 41);
            this.Venta.TabIndex = 0;
            this.Venta.Text = "Venta";
            this.Venta.UseVisualStyleBackColor = true;
            // 
            // Entrada
            // 
            this.Entrada.Location = new System.Drawing.Point(523, 207);
            this.Entrada.Name = "Entrada";
            this.Entrada.Size = new System.Drawing.Size(159, 41);
            this.Entrada.TabIndex = 1;
            this.Entrada.Text = "Entrada";
            this.Entrada.UseVisualStyleBackColor = true;
            // 
            // Deuda
            // 
            this.Deuda.Location = new System.Drawing.Point(523, 254);
            this.Deuda.Name = "Deuda";
            this.Deuda.Size = new System.Drawing.Size(159, 41);
            this.Deuda.TabIndex = 2;
            this.Deuda.Text = "Deuda";
            this.Deuda.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(855, 583);
            this.Controls.Add(this.Deuda);
            this.Controls.Add(this.Entrada);
            this.Controls.Add(this.Venta);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button Venta;
        private System.Windows.Forms.Button Entrada;
        private System.Windows.Forms.Button Deuda;
    }
}

