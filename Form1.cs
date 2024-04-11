using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace GameBai
{
    public partial class Form1 : Form
    {
        private List<card> deck;
        private const int NUM_CARDS_PER_PLAYER = 10;
        SocketManager socket = new SocketManager(); 
        private ImageList imageList = new ImageList();
        private List<card> Player1Hand = new List<card>();
        private List<card> Player2Hand = new List<card>();
        private List<card> ClientHand = new List<card>();
        private Dictionary<Button, card> btncard = new Dictionary<Button,card>(); 
        private bool isPlayer1Turn = true;
        public Form1()
        {
            InitializeComponent();
            InitializeDeck();
        }
        private void InitializeDeck()
        {
            deck = new List<card>();
            // Create a deck of cards
            for (int suit = 1; suit <= 4; suit++)
            {
                for (int value = 2; value <= 14; value++)
                {
                    deck.Add(new card { Suit = suit, Value = value });
                }
            }
            // Shuffle the deck
            Shuffle(deck);
        }
        private void AddImageToList(string imagePath)
        {
            try
            {
                Image image = Image.FromFile(imagePath);
                ptb.Image = image;
        
                imageList.Images.Add(Path.GetFileName(imagePath), image);
        
                ListViewItem item = new ListViewItem();
                item.ImageKey = Path.GetFileName(imagePath);
                listView1.Items.Add(item);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading image: {ex.Message}");
            }
        }
        private void DealCards()
        {
            
            for (int i = 0; i < NUM_CARDS_PER_PLAYER; i++)
            {
                card card = deck[i];
                Player1Hand.Add(card);
                Button button = Controls["b" + (i + 1)] as Button;
                btncard.Add(button, card);
                SetButtonImage(button, card.ImageName);
            }
            for (int i = NUM_CARDS_PER_PLAYER; i < NUM_CARDS_PER_PLAYER * 2; i++)
            {
                card card = deck[i];
                Player2Hand.Add(card);
                Button button = Controls["b" + (i + 1)] as Button;
                btncard.Add(button, card);
                //SetButtonImage(button, card.ImageName);
            }
        }
        
        private void SetButtonImage(Button button, string imagePath)
        {
            try
            {
                // Load image from file
                Image image = Image.FromFile(imagePath);
                button.BackgroundImage = image;
            }
            catch (FileNotFoundException)
            {
                MessageBox.Show("Image not found: " + imagePath);
            }
        }

        // Fisher-Yates shuffle algorithm
        private void Shuffle<T>(List<T> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            string v = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            txtip.Text = v;
            if (string.IsNullOrEmpty(txtip.Text))
            {
                txtip.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }
        public void updateClientHand()
        {
            for (int i = 0; i < ClientHand.Count; i++)
            {
                Button button = Controls["b" + (i+1)] as Button;
                btncard.Add(button, ClientHand[i]);
                SetButtonImage(button, ClientHand[i].ImageName);

            }
        }
        public void checkWin(List<card> cards)
        {
            if(cards.Count == 0)
            {
                MessageBox.Show("Kết thúc game");
            }
        }
        private void ProcessData(SocketData data)
        {
            switch(data.Command)
            {
                case (int)SocketCommand.NOTIFY:
                    MessageBox.Show(data.Message);
                    break;
                case (int)SocketCommand.NEW_GAME:
                    break;
                case (int)SocketCommand.SEND_CARD:
                    ClientHand.Add(data.Cards);
                    break;
                case (int)SocketCommand.QUIT: 
                    break;
                case (int)SocketCommand.SEND_HAND:
                    updateClientHand();
                    break;
                case (int)SocketCommand.SEND_PLAYCARD:
                    Image img = Image.FromFile(data.Cards.ImageName);
                    ptb.BackgroundImage = img;
                    break;
                default:
                    break;
            }
            Listen();
        }
        private void btnlan_Click(object sender, EventArgs e)
        {
            socket.IP = txtip.Text;
            if (!socket.ConnectServer())
            {
                socket.CreateServer();
            }
            else
            {
                Listen();
            }
        }
        void Listen()
        {
                Thread listenthread = new Thread(() =>
                {
                    try
                    {
                        SocketData data = (SocketData)socket.Receive();
                        ProcessData(data);
                    }
                    catch
                    {

                    }
                });
                listenthread.IsBackground = true;
                listenthread.Start();
        }

        private void chia_Click(object sender, EventArgs e)
        {
            DealCards();
            foreach (var item in Player2Hand)
            {
                try
                {
                    socket.Send(new SocketData((int)SocketCommand.SEND_CARD, "", item));
                    Listen();
                }
                catch
                {
                }
            }
            socket.Send(new SocketData((int)SocketCommand.SEND_HAND, "", null));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            b1.Click += new EventHandler(Button_Click);
            b2.Click += new EventHandler(Button_Click);
            b3.Click += new EventHandler(Button_Click);
            b4.Click += new EventHandler(Button_Click);
            b5.Click += new EventHandler(Button_Click);
            b6.Click += new EventHandler(Button_Click);
            b7.Click += new EventHandler(Button_Click);
            b8.Click += new EventHandler(Button_Click);
            b9.Click += new EventHandler(Button_Click);
            b10.Click += new EventHandler(Button_Click);

            b11.Click += new EventHandler(Button_Click);
            b12.Click += new EventHandler(Button_Click);
            b13.Click += new EventHandler(Button_Click);
            b14.Click += new EventHandler(Button_Click);
            b15.Click += new EventHandler(Button_Click);
            b16.Click += new EventHandler(Button_Click);
            b17.Click += new EventHandler(Button_Click);
            b18.Click += new EventHandler(Button_Click);
            b19.Click += new EventHandler(Button_Click);
            b20.Click += new EventHandler(Button_Click);
        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null && btncard.ContainsKey(clickedButton))
            {
                card cardToAdd = btncard[clickedButton];
                AddImageToList(cardToAdd.ImageName);
                foreach(var card in Player1Hand)
                {
                    if(card.ImageName == cardToAdd.ImageName)
                    {
                        Player1Hand.Remove(card);
                        Image image = Image.FromFile("E:\\Code\\AI\\GameBai\\images\\closedCard.png"); 
                        socket.Send(new SocketData((int)SocketCommand.SEND_PLAYCARD, "", card));
                        clickedButton.BackgroundImage = image;
                        clickedButton.Enabled = false;
                        checkWin(Player1Hand);
                        checkWin(ClientHand);
                        break;
                    }
                }
            }
        }

        private void btndanh_Click(object sender, EventArgs e)
        {
            listView1.View = View.LargeIcon;
            listView1.GridLines = true;
            listView1.LargeImageList = new ImageList();
            listView1.LargeImageList.ImageSize = new Size(100, 100);

            imageList = new ImageList();
            imageList.ImageSize = new Size(90, 90);
            listView1.LargeImageList = imageList;

            // Thêm ảnh vào ImageList và hiển thị trong ListView
            //AddImageToList("E:\\Code\\AI\\GameBai\\images\\card2_4.png");
            //AddImageToList("E:\\Code\\AI\\GameBai\\images\\card2_5.png");
            //AddImageToList("E:\\Code\\AI\\GameBai\\images\\card2_6.png");

        }

        private void btnbq_Click(object sender, EventArgs e)
        {
            foreach (var item in ClientHand)
            {
                MessageBox.Show(item.ImageName);
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
