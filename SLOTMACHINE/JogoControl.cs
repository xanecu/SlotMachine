using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SLOTMACHINE
{
    /*DELEGATE: Apontador de metodos; Delegate = metodo void
      Delegate tem o mesmo nivel que uma Classe
     */

    //1. Declarar delegate para tipificar o evento OnFalencia
    public delegate void OnFalencia_Handler(Object sender, EventArgs args);

    //1. Para disparar o evento OnPremio => Implementar delegate
    public delegate void OnPremio_Handler(object sender, OnPremio_Args args);

    //INTERFACE permite criar regras para eventos, delegates ou propriedades
    interface IJogo
    {
        event OnFalencia_Handler OnFalencia;
        event OnPremio_Handler OnPremio;
        /*
        Dentro posso por propriedades
        interface pode levar assinaturas de properties,
        assinaturas de metodos e de delegates.
        Não pode levar variaveis
        */
        int Diamante1 { get; set; }
        int Diamante2 { get; set; }
        int Diamante3 { get; set; }

        /*Neste caso não existe OVERLOADING (definir duas vezes um método com o mesmo
        nome mas com a assinatura diferente)
        */
        void Jogar(int aposta);


    }


    //Declaramos a Classe Public para que a MainWindow possa ter acesso ao jogo
    public abstract class JogoControl
    {
        public abstract void Add(int add);

        //Cte posta em maiuscula para identificação
        public const int MAXAPOSTA = 100;


        //Construtores permitem a inicialização do jogo
        public JogoControl() //construtor 1
        {
            this._balance = 0;
        }

        public JogoControl(int balance) //construtor2
        {
            this.Balance = balance;
        }

        protected int _balance; //campo privado/protegido

        public int Balance //Property que só irá atualizar se o valor for igual ou maior que zero
        {
            get { return _balance; } //Lógica para acesso à leitura

            set
            {
                if (value >= 0) this._balance = value;
            }
            //É atraves do "set" que o cliente atualiza o campo
            //logica para acesso a escritura
        }

    }

    //Classe de argumento
    public class OnPremio_Args : EventArgs
    {
        public int bet;
        public DateTime quando;
        public OnPremio_Args(int ap)
        {
            this.bet = ap;
            quando = DateTime.Now;
        }
    }


    public class JogoDiamantes : JogoControl, IJogo
    {
        public override void Add(int add)
        {
            Balance += add;
        }

        //Especializamos introduzindo um array privado de 3 inteiros
        int[] _diamantes = new int[3] { 1, 2, 3 };


        //2 CONSTRUTORES que chamam a Classe mae JogoDiamantes
        public JogoDiamantes() : base()//Construtor 1
        {

        }

        public JogoDiamantes(int balance) : base(balance)//Construtor 2
        {
            //Gera a imagem dos diamantes "random" ao abrir a App
            Random r = new Random();
            Diamante1 = r.Next(1, 6);
            Diamante2 = r.Next(1, 6);
            Diamante3 = r.Next(1, 6);
        }

        //Property indice (i)
        public int this[int i]
        {
            get
            {
                switch (i)
                {
                    case (0):
                        return _diamantes[0];
                    case (1):
                        return _diamantes[1];
                    case (2):
                        return _diamantes[2];
                    default:
                        throw new Exception("Diamante invalido");

                }
            }

            set
            {
                if (value >= 1 && value <= 5)
                {
                    switch (i)
                    {
                        case (0):
                            _diamantes[0] = value;
                            break;
                        case (1):
                            _diamantes[1] = value;
                            break;
                        case (2):
                            _diamantes[2] = value;
                            break;
                        default:
                            throw new Exception("Diamante invalido");
                    }
                }
            }
        }

        //2 Propriedades provenientes do interface para aplicar regras em cada imagem
        public int Diamante1
        {
            get { return _diamantes[0]; }//1ªpossição do array;
            set { if (value >= 1 && value <= 6) this._diamantes[0] = value; }
        }

        public int Diamante2
        {
            get { return _diamantes[1]; }//2ªpossição de array;
            set { if (value >= 1 && value <= 6) this._diamantes[1] = value; }
        }

        public int Diamante3
        {
            get { return _diamantes[2]; }//3ª possição do array;
            set { if (value >= 1 && value <= 6) this._diamantes[2] = value; }
        }

        /*
         O botão tem definido um delegate que a assinatura do método vai reagir.
         Esse metodo tem o nome de Event Handler
         */

        //2. Declaração do evento OnFalencia
        public event OnFalencia_Handler OnFalencia;

        //2. Declaração do evento OnPremio
        public event OnPremio_Handler OnPremio;

        public void Jogar(int bet)
        {
            if (Balance >= bet)
            {
                Balance -= bet;
                //3. Disparar o evento OnFalencia se..
                if (Balance == 0 && OnFalencia != null) OnFalencia(this, new EventArgs());
            }
            else if (Balance > 0)
            {
                bet = Balance;
                Balance = 0;
                if (OnFalencia != null) OnFalencia(this, new EventArgs());
            }
            else bet = 0;

            if (bet > 0)
            {
                //Se tudo ok, núm aleatorio entre 1 e 5 nos diamantes
                Random r = new Random();
                Diamante1 = r.Next(1, 6);
                Diamante2 = r.Next(1, 6);
                Diamante3 = r.Next(1, 6);
                //3. Disparar o evento OnPremio se.. (*)
                //As tres imagens iguais: BET*n*2
                if (Diamante1 == Diamante2 && Diamante2 == Diamante3)
                {
                    if (OnPremio != null) OnPremio(this, new OnPremio_Args(bet));
                }
                //(*) Duas imagens iguais juntas na 1ª e 2ª pos.: BET*n*(2/2=1)
                else if (Diamante1 == Diamante2)
                {

                    if (OnPremio != null) OnPremio(this, new OnPremio_Args(bet / 2));

                }
                //(*) Duas imagens iguais juntas na 2ª e 3ª pos.: "
                else if (Diamante2 == Diamante3)
                {
                    if (OnPremio != null) OnPremio(this, new OnPremio_Args(bet / 2));
                }
            }
        }



    }








}
