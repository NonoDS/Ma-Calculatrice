using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Calculateur;

namespace CalculatriceFR2
{
    public partial class Chaos : Form
    {
        public Chaos()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Calcul l'expression donnée en paramètre
        /// </summary>
        /// <param name="expression">Expression à calculer</param>
        /// <returns></returns>
        private string CalulerExpression(string expression)
        {
            try
            {
                double resultat = _calcul.CalcluerExpression(expression);
                return expression + " = " + resultat.ToString(System.Globalization.CultureInfo.CurrentUICulture); //L'expression s'est calculée
            }
            catch(Exception exp)
            {
                MessageBox.Show(this
                    , exp.Message
                    , IHM.ResourceManager.GetString("MessageBoxErreur")
                    , MessageBoxButtons.OK
                    , MessageBoxIcon.Warning);
                return null; //Erreur de calcul de l'expression
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            _calcul.OnAjoutFonction += new EventHandler<FonctionEventArgs>(this.Form1_AjoutFonction);
            _calcul.OnSuppFonction += new EventHandler<FonctionEventArgs>(this.Form1_SuppFonction);
            _calcul.OnAjoutVariable += new EventHandler<VariableEventArgs>(this.Form1_MAJVariables);
            //mCalcul.OnSuppVariable += new Variables.MAJVariable(this.Form1_MAJVariables);
            System.Windows.Forms.ToolStripMenuItem eltCourant;
            foreach (int i in Enum.GetValues(typeof(Calculateur.FonctionTrigo.FCTrigo)))
            {
                eltCourant = new System.Windows.Forms.ToolStripMenuItem(((Calculateur.FonctionTrigo.FCTrigo)i).ToString());
                fonctionsToolStripMenuItem.DropDownItems.Add(eltCourant);
                eltCourant.Click += new EventHandler(FcontionTrigo_Click);
            }
            _calcul.LesVariables.Ajouter("PI", Math.PI);
            _calcul.LesVariables.Ajouter("E", Math.E);
        }

        /// <summary>
        /// Traitement du clic sur une fonction trigo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FcontionTrigo_Click(object sender, EventArgs e)
        {
            int positionCourante = TXT_Expression.SelectionStart;
            string fonction = ((System.Windows.Forms.ToolStripMenuItem)sender).Text + "(";
            TXT_Expression.Text = TXT_Expression.Text.Insert(TXT_Expression.SelectionStart, fonction);
            TXT_Expression.SelectionStart = positionCourante + fonction.Length;
        }

        /// <summary>
        /// Mise à jour de l'IHM pour mettre à jour l'IHM pour afficher la nouvelle variable
        /// </summary>
        /// <param name="variable"></param>
        private void Form1_MAJVariables(object sender, VariableEventArgs variable)
        {
            ListeVariables.Items.Remove(variable.Variable);
            ListeVariables.Items.Add(variable.Variable);
        }

        /// <summary>
        /// Mise à jour de l'IHM pour afficher la nouvelle fonction
        /// </summary>
        /// <param name="fonction"></param>
        private void Form1_AjoutFonction(object sender, FonctionEventArgs fonction)
        {
            fonction.Fonction.Expression = TXT_Expression.Text;
            ListeFonctions.Items.Add(fonction.Fonction);
        }

        private void Form1_SuppFonction(object sender, FonctionEventArgs fonction)
        {
            ListeFonctions.Items.Remove(fonction.Fonction);
        }

        #region Attributs privés
        CalculateurIhm _calcul = new CalculateurIhm();
        #endregion


        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new AboutBox1().ShowDialog();
        }

        private void TXT_Expression_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                ExecuterCalcul();
            }
        }

        /// <summary>
        /// Execution du calcul
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_Exec_Click(object sender, EventArgs e)
        {
            ExecuterCalcul();
        }
        
        /// <summary>
        /// Execution du calcul
        /// </summary>
        private void ExecuterCalcul()
        {
            try
            {
                ListeCalculs.AppendText(TXT_Expression.Text + " = " + _calcul.CalcluerExpression(TXT_Expression.Text) + "\r\n");
                TXT_Expression.Text = string.Empty;
            }
            catch (Exception exp)
            {
                MessageBox.Show(this, exp.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// L'utilisateur a fait une demande de changement de type d'angle : Passage en degres
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_Degres_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                _calcul.Angle = TypeAngle.Degres;
            }
        }

        /// <summary>
        /// L'utilisateur a fait une demande de changement de type d'angle : Passage en radians
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_Radians_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                _calcul.Angle = TypeAngle.Radians;
            }
        }

        /// <summary>
        /// L'utilisateur a fait une demande de changement de type d'angle : Passage en grades
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radio_Grades_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                _calcul.Angle = TypeAngle.Grades;
            }
        }

        /// <summary>
        /// L'utilisateur a fait une demande d'effacement de la liste des calculs
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BTN_ClearCalculs_Click(object sender, EventArgs e)
        {
            ListeCalculs.Clear();
        }
    }
}