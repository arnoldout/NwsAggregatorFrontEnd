﻿using NewsAggregator.Data;
using NewsAggregator.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace NewsAggregator
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NwsViewer : Page
    {
        public String uri;
        public NwsViewer()
        {
            this.InitializeComponent();
            backArrow.Source= new BitmapImage(new Uri(this.BaseUri, "/Assets/back_Arrow.png"));
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = (Story)e.Parameter;
            uri = parameters.Uri;
            uri = uri + "?smid=fb-nytimes&smtyp=cur";
            viewer.Navigate(new Uri(uri));
            AutoResetEvent waitForNavComplete = new AutoResetEvent(false);
            progressRing.IsActive = true;
            await Task.Run(() => { waitForNavComplete.WaitOne(); });
            progressRing.IsActive = false;
            waitForNavComplete.Reset();
            headerText.Text = parameters.Title;
            foreach (String s in parameters.Categories)
            {
                StoryService.addLike(s, App.loginid);
            }
        }

        private void viewer_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            if(args.Uri!=null&&args.Uri != new Uri(uri))
            {
                //args.Cancel = true;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}
