﻿using Xamarin.Forms.Internals;
using Xamarin.Forms.CustomAttributes;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls
{
#if UITEST
	[Category(UITestCategories.CollectionView)]
#endif
	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Github, 10735, "[Bug] [Fatal] [Android] CollectionView Causes Application Crash When Keyboard Opens", PlatformAffected.Android)]
	public partial class Issue10735 : TestContentPage
	{
		readonly int _addItemDelay = 300;
		readonly int _changeFocusDelay = 1000;
		int _item = 0;
		View _lastFocus;

		public Issue10735()
		{
#if APP
			InitializeComponent();
            BindingContext = this;
            StartAddingMessages();
#endif
        }

        public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();

        protected override void Init()
		{

        }

		void StartAddingMessages()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(_addItemDelay);
                    Items.Add(_item.ToString());
                    _item++;
                }
            });
#if APP
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(_changeFocusDelay);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        _lastFocus?.Unfocus();

                        if (_lastFocus == _editor)
                            _lastFocus = _button;
                        else
                            _lastFocus = _editor;
                        
                        _lastFocus.Focus();
                    });
                }
            });
#endif
        }
    }
}