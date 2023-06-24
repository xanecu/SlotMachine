using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Windows.Diagnostics;

namespace SLOTMACHINE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        JogoDiamantes j; //declaro variavel objeto

        public MainWindow()
        {
            InitializeComponent();

            j = new JogoDiamantes(0); // instancio a variavel

            //4. Registo dos Eventos OnFalencia e OnPremio
            j.OnFalencia += J_OnFalencia;
            j.OnPremio += J_OnPremio;

            j[0] = 5;
            j[1] = 2;
            j[2] = 1;
            Ver(j);

        }

        protected void Ver(JogoDiamantes jog)
        {
            this.txtbalance.Text = jog.Balance.ToString();
            this.img1.Source = new BitmapImage(new Uri("Diamonds/"
                + jog.Diamante1.ToString() + ".jpg", UriKind.Relative));
            this.img2.Source = new BitmapImage(new Uri("Diamonds/"
                + jog.Diamante2.ToString() + ".jpg", UriKind.Relative));
            this.img3.Source = new BitmapImage(new Uri("Diamonds/"
                + jog.Diamante3.ToString() + ".jpg", UriKind.Relative));

        }

        //5. Declarar e implementar o Event Handler, método que reage ao evento OnPremio
        private void J_OnPremio(object sender, OnPremio_Args args)
        {//String Builder, como se fosse um array de strings
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            JogoDiamantes s = (JogoDiamantes)sender;//UNBOXING
            int premio = args.bet * 2 * s.Diamante2;
            s.Balance += premio;
            sb.Append("FELICIDADES JOGADOR/A!" + "\n" +
                      "GANHOU " + premio.ToString() + "€!");
            this.visorpremio.Text = sb.ToString();
        }
        
        //5. Declarar e implementar o Event Handler (tratador do evento)
        private void J_OnFalencia(object sender, EventArgs args)
        {
            this.visorpremio.Text = "GAME OVER";
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            int add;
            if (int.TryParse(txtdeposit.Text, out add))
            {
                j.Add(add);
                Ver(j);
            }
            
        }

        protected void ver()
        {
            if(j != null)
            {
                this.txtbalance.Text = j.Balance.ToString();
            }
        }

        private void spin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.visorpremio.Text = "";
                int bet;
                if (int.TryParse(txtbet.Text, out bet))
                {
                    j.Jogar(bet);
                    Ver(j);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
