using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
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
using MySql.Data.MySqlClient;

namespace Your_Turn_Client
{
    public partial class MainWindow : Window
    {
        bool isLoginInputID, isLoginInputPassword, isPossibleLogin = false;
        bool isRegisterInputID, isRegisterInputPassword, isRegisterInputCheckPassword, isRegisterInputNickName, isSamePassword, isCanUseThisNickName, isCanUseThisID,  isPossibleRegister = false;
        Scripts.DB DB = new Scripts.DB();
        Scripts.FileManager File = new Scripts.FileManager();

        public MainWindow()
        {
            InitializeComponent();
            if (File.ReadAutoLoginValue().Equals("True"))
                AutoLogin();
        }


        private void InputID_Changed(object sender, KeyEventArgs e)
        {
            WindowUpdate();
            if (LoginPanel.Visibility == Visibility.Visible)
            {
                isLoginInputID = LoginIDTextBox.Text == "" ? false : true;
                CheckReadyToLogin();
            }
            else
            {
                isRegisterInputID = RegisterIDTextBox.Text == "" ? false : true;
                CheckReadyToRegister();
            }
        }

        private void InputPassword_Changed(object sender, KeyEventArgs e)
        {
            WindowUpdate();
            if (LoginPanel.Visibility == Visibility.Visible)
            {
                isLoginInputPassword = LoginPasswordTextBox.Password == "" ? false : true;
                CheckReadyToLogin();
            }
            else
            {
                isRegisterInputPassword = RegisterPasswordTextBox.Password == "" ? false : true;
                CheckReadyToRegister();
            }
        }

        void WindowUpdate()
        {
            System.Windows.Threading.Dispatcher.CurrentDispatcher.Invoke(
                System.Windows.Threading.DispatcherPriority.Background,
                new ThreadStart(delegate { }));
            Console.WriteLine(isPossibleLogin);
        }

        private void ToLoginPanel(object sender, RoutedEventArgs e)
        {
            GoToLoginPanel();
        }
        private void ToRegisterPanel(object sender, RoutedEventArgs e)
        {
            GoToRegisterPanel();
        }

        void GoToLoginPanel()
        {
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;
            RegisterIDTextBox.Text = "";
            RegisterPasswordTextBox.Password = "";
            RegisterPasswordCheckTextBox.Password = "";
            RegisterPasswordCheckText.Text = "";
            RegisterNickNameTextBox.Text = "";
            CanUseThisNickNameText.Text = "";
            CanUseThisIDText.Text = "";
            isRegisterInputID = false;
            isRegisterInputPassword = false;
            isRegisterInputCheckPassword = false;
            isRegisterInputNickName = false;
            isSamePassword = false;
            isCanUseThisNickName = false;
            isCanUseThisID = false;
            isPossibleRegister = false;
            RegisterButtonImage.Source = new BitmapImage(new Uri("Image\\RegisterImpossibleButton.png", UriKind.Relative));
            WindowUpdate();
        }

        void GoToRegisterPanel()
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
            LoginIDTextBox.Text = "";
            LoginPasswordTextBox.Password = "";
            isLoginInputID = false;
            isLoginInputPassword = false;
            isPossibleLogin = false;
            LoginButtonImage.Source = new BitmapImage(new Uri("Image\\LoginImpossibleButton.png", UriKind.Relative));

        }

        private void LogoutButtonClick(object sender, RoutedEventArgs e)
        {
            GoToRegisterPanel();
            AutoLoginCheckBox.IsChecked = false;
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;
            PlayPanel.Visibility = Visibility.Collapsed;
            LoginBeforeBackGroundImage.Visibility = Visibility.Visible;
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            if (isPossibleLogin)
            {
                string TryString = DB.TryLogin(LoginIDTextBox.Text, LoginPasswordTextBox.Password);
                if (TryString.Equals("로그인 성공"))
                   {
                    File.WriteAutoLoginValue(AutoLoginCheckBox.IsChecked, DB.GetNickName(LoginIDTextBox.Text, LoginPasswordTextBox.Password));
                    File.WriteNickName(DB.GetNickName(LoginIDTextBox.Text, LoginPasswordTextBox.Password));
                    LoginPanel.Visibility = Visibility.Collapsed;
                    RegisterPanel.Visibility = Visibility.Collapsed;
                    PlayPanel.Visibility = Visibility.Visible;
                    ShowIDText.Text = string.Format("{0}님, 환영합니다.", DB.GetNickName(LoginIDTextBox.Text, LoginPasswordTextBox.Password));
                    LoginBeforeBackGroundImage.Visibility = Visibility.Collapsed;
                }
                else
                    MessageBox.Show(TryString);
            }
        }

        void AutoLogin()
        {
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Collapsed;
            PlayPanel.Visibility = Visibility.Visible;
            ShowIDText.Text = string.Format("{0}님, 환영합니다.", File.ReadNickNameValue());
            LoginBeforeBackGroundImage.Visibility = Visibility.Collapsed;
        }

        private void RegisterButtonClick(object sender, RoutedEventArgs e)
        {
            if(isPossibleRegister)
            {
                Console.WriteLine("회원가입 가능");
                if (DB.InsertUser(RegisterIDTextBox.Text, RegisterPasswordTextBox.Password, RegisterNickNameTextBox.Text) == 1)
                {
                    MessageBox.Show("회원가입 성공");
                    GoToLoginPanel();
                }
                else
                    MessageBox.Show("오류가 발생하였습니다.");
            }
        }

        private void CheckNickNameOverlap(object sender, RoutedEventArgs e)
        {
            if (RegisterNickNameTextBox.Text.Equals(""))
            {
                CanUseThisNickNameText.Text = "닉네임을 입력해 주세요.";
                CanUseThisNickNameText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
                isCanUseThisNickName = false;
                return;
            }

            if (DB.CheckOverlapNickName(RegisterNickNameTextBox.Text))
            {
                CanUseThisNickNameText.Text = "이 닉네임은 사용 불가능합니다.";
                CanUseThisNickNameText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE53935"));
                isCanUseThisNickName = false;
            }
            else
            {
                CanUseThisNickNameText.Text = "이 닉네임은 사용 가능합니다.";
                CanUseThisNickNameText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF76FF03"));
                isCanUseThisNickName = true;
            }
            CheckReadyToRegister();
        }

        private void CheckIdOverlap(object sender, RoutedEventArgs e)
        {
            if (RegisterIDTextBox.Text.Equals(""))
            {
                CanUseThisIDText.Text = "아이디을 입력해 주세요.";
                CanUseThisIDText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF000000"));
                isCanUseThisID = false;
                return;
            }

            if (DB.CheckOverlapID(RegisterIDTextBox.Text))
            {
                CanUseThisIDText.Text = "이 아이디은 사용 불가능합니다.";
                CanUseThisIDText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE53935"));
                isCanUseThisID = false;
            }
            else
            {
                CanUseThisIDText.Text = "이 아이디은 사용 가능합니다.";
                CanUseThisIDText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF76FF03"));
                isCanUseThisID = true;
            }
            CheckReadyToRegister();
        }


        private void InputCheckPassword_Changed(object sender, KeyEventArgs e)
        {
            WindowUpdate();
            isRegisterInputCheckPassword = RegisterPasswordCheckTextBox.Password == "" ? false : true;
            if (RegisterPasswordCheckTextBox.Password.Equals(RegisterPasswordTextBox.Password))
            {
                RegisterPasswordCheckText.Text = "일치";
                RegisterPasswordCheckText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF76FF03"));
            }
            else if (RegisterPasswordCheckTextBox.Password == "")
                RegisterPasswordCheckText.Text = "";
            else
            {
                RegisterPasswordCheckText.Text = "불일치";
                RegisterPasswordCheckText.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE53935"));
            }
            isSamePassword = RegisterPasswordCheckText.Text.Equals("일치") ? true : false;
            CheckReadyToRegister();
        }

        private void InputNickName_Changed(object sender, KeyEventArgs e)
        {
            WindowUpdate();
            isRegisterInputNickName = RegisterNickNameTextBox.Text == "" ? false : true;
            CheckReadyToRegister();
        }

        void CheckReadyToLogin()
        {
            LoginButtonImage.Source = (isLoginInputPassword && isLoginInputID) ? 
                new BitmapImage(new Uri("Image\\LoginPossibleButton.png", UriKind.Relative)) : new BitmapImage(new Uri("Image\\LoginImpossibleButton.png", UriKind.Relative));
            isPossibleLogin = (isLoginInputPassword && isLoginInputID) ? true : false;
        }

        void CheckReadyToRegister()
        {
           RegisterButtonImage.Source =
                (isRegisterInputID && isRegisterInputPassword && isRegisterInputCheckPassword && isRegisterInputNickName && isSamePassword && isCanUseThisNickName && isCanUseThisID) ?
                new BitmapImage(new Uri("Image\\RegisterPossibleButton.png", UriKind.Relative)) : new BitmapImage(new Uri("Image\\RegisterImpossibleButton.png", UriKind.Relative));
            isPossibleRegister = (isRegisterInputID && isRegisterInputPassword && isRegisterInputCheckPassword && isRegisterInputNickName && isSamePassword && isCanUseThisNickName && isCanUseThisID) ?
                true : false;
        }
    }
}
