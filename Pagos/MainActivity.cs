using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Stripe;
using Com.Braintreepayments.Cardform.View;
using System.Net.Http;
using Newtonsoft.Json;
using ProyectoComub;
using System.Text;

namespace Pagos
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        CardForm cardForm;
        Button btnPagar;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            cardForm = FindViewById<CardForm>(Resource.Id.card_form);
            btnPagar = FindViewById<Button>(Resource.Id.btnBuy);
            cardForm.CardRequired(true).ExpirationRequired(true).CvvRequired(true).Setup(this);
            cardForm.CvvEditText.SetRawInputType(Android.Text.InputTypes.ClassNumber | Android.Text.InputTypes.NumberVariationPassword);
            btnPagar.Click += BtnPagar_Click;

        }

        private async void BtnPagar_Click(object sender, System.EventArgs e)
        {
            HttpResponseMessage res;
            if (cardForm.IsValid)
            {
                try
                {
                    HttpClient client = new HttpClient();
                    res = await client.PostAsync("https://www.alex-pruebas.somee.com/api/values/", new StringContent(JsonConvert.SerializeObject(new PaymentModel()
                    {
                        Amount = 100,
                        Token = CreateToken(cardForm.CardNumber, long.Parse(cardForm.ExpirationMonth), long.Parse(cardForm.ExpirationYear), cardForm.Cvv)
                    }),
                    Encoding.UTF8,"application/json"
                    ));
                    Toast.MakeText(this, "Hasta aqui todo bien xd", ToastLength.Long).Show();
                    System.Console.WriteLine(res);
                }
                catch (System.Exception ex)
                {
                    Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
                }

            }
            else
                Toast.MakeText(this, "Complete los campos", ToastLength.Long).Show();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        public string CreateToken(string cardNumber, long cardExpMonth, long cardExpYear, string cardCVC)
        {
            StripeConfiguration.ApiKey = "pk_test_HTHqbRWI3sZPO77F8TfXZ3Lx00zCrtZCHi";

            var tokenOptions = new TokenCreateOptions()
            {
                Card = new CreditCardOptions()
                {
                    Number = cardNumber,
                    ExpMonth = cardExpMonth,
                    ExpYear = cardExpYear,
                    Cvc = cardCVC
                }
            };

            var tokenService = new TokenService();
            Token stripeToken = tokenService.Create(tokenOptions);

            return stripeToken.Id; // This is the token
        }
    }
}