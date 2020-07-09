﻿using System.Collections.Generic;
using System.Linq;
using Uno.Extensions;
using Uno.Extensions.Specialized;
using Uno.Material.Helpers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Uno.Material.Controls
{
    [TemplatePart(Name = LayoutRootName, Type = typeof(Grid))]
	public partial class BottomNavigationBar : Control
	{
		private const string LayoutRootName = "PART_LayoutRoot";

		private bool _initialized = false;
		private Grid _layoutRoot;

		public BottomNavigationBar()
		{
			DefaultStyleKey = typeof(BottomNavigationBar);
			Items = new List<BottomNavigationBarItem>();

			Unloaded += BottomNavigationBar_Unloaded;
		}

		#region Property: Items

		public static DependencyProperty ItemsProperty { get; } = DependencyProperty.Register(
			nameof(Items),
			typeof(IList<BottomNavigationBarItem>),
			typeof(BottomNavigationBar),
			new PropertyMetadata(default, OnItemsChanged));
		public IList<BottomNavigationBarItem> Items
		{
			get => (IList<BottomNavigationBarItem>)GetValue(ItemsProperty);
			set => SetValue(ItemsProperty, value);
		}

		#endregion
		#region Property: SelectedItem

		public static DependencyProperty SelectedItemProperty { get; } = DependencyProperty.Register(
			nameof(SelectedItem),
			typeof(BottomNavigationBarItem),
			typeof(BottomNavigationBar),
			new PropertyMetadata(default, OnSelectedItemChanged));
		public BottomNavigationBarItem SelectedItem
		{
			get => (BottomNavigationBarItem)GetValue(SelectedItemProperty);
			set => SetValue(SelectedItemProperty, value);
		}

		#endregion

		protected override void OnApplyTemplate()
		{
			base.OnApplyTemplate();

			_layoutRoot = this.GetTemplateChild<Grid>(GetTemplateChild, LayoutRootName);
			_initialized = true;

			GenerateTabItems();
		}

		internal void GenerateTabItems()
		{
			if (!_initialized) return;
			if (_layoutRoot != null)
			{
				// todo: diff update
				_layoutRoot.ColumnDefinitions.Clear();
				_layoutRoot.Children.Clear();

				if (Items != null)
				{
					foreach (var item in Items)
					{
						_layoutRoot.ColumnDefinitions.Add(new ColumnDefinition());
						_layoutRoot.Children.Add(item);
						Grid.SetColumn(item, _layoutRoot.Children.Count - 1);
					}

					RegisterBarItemsEvents();

					SelectedItem = Items.FirstOrDefault(x => x.IsChecked == true) ?? Items.FirstOrDefault();
				}
				else
				{
					SelectedItem = null;
				}
			}
		}

		private void BottomNavigationBar_Unloaded(object sender, RoutedEventArgs e)
		{
			Unloaded -= BottomNavigationBar_Unloaded;

			UnregisterBarItemsEvents();
		}

		private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			(d as BottomNavigationBar).GenerateTabItems();
		}

		private static void OnSelectedItemChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (e.NewValue is BottomNavigationBarItem item)
			{
				item.IsChecked = true;
			}
		}

		private void BottomNavigationBarItem_Checked(object sender, RoutedEventArgs e)
		{
			var navItem = sender as BottomNavigationBarItem;

			SelectedItem = navItem;

			foreach (BottomNavigationBarItem item in Items.Where(i => !i.Equals(navItem)))
			{
				item.IsChecked = false;
			}
		}

		private void BottomNavigationBarItem_Unchecked(object sender, RoutedEventArgs e)
		{
			// We make sure to not unselect the currently selected item
			if (sender is BottomNavigationBarItem item && item == SelectedItem)
			{
				item.IsChecked = true;
			}
		}

		private void RegisterBarItemsEvents()
		{
			foreach (var item in Items.Safe())
			{
				item.Checked += BottomNavigationBarItem_Checked;
				item.Unchecked += BottomNavigationBarItem_Unchecked;
			}
		}

		private void UnregisterBarItemsEvents()
		{
			foreach (var item in Items.Safe())
			{
				item.Checked -= BottomNavigationBarItem_Checked;
				item.Unchecked -= BottomNavigationBarItem_Unchecked;
			}
		}
	}
}
