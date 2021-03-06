﻿// SeaBotCore
// Copyright (C) 2018 - 2019 Weespin
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//  
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
namespace SeaBotCore.Utils
{
    #region

    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Collections.Specialized;
    using System.ComponentModel;

    #endregion

    public class FullyObservableCollection<T> : ObservableCollection<T>
        where T : INotifyPropertyChanged
    {
        public FullyObservableCollection()
        {
        }

        public FullyObservableCollection(List<T> list)
            : base(list)
        {
            this.ObserveAll();
        }

        public FullyObservableCollection(IEnumerable<T> enumerable)
            : base(enumerable)
        {
            this.ObserveAll();
        }

        /// <summary>
        ///     Occurs when a property is changed within an item.
        /// </summary>
        public event EventHandler<ItemPropertyChangedEventArgs> ItemPropertyChanged;

        protected override void ClearItems()
        {
            foreach (var item in this.Items)
            {
                item.PropertyChanged -= this.ChildPropertyChanged;
            }

            base.ClearItems();
        }

        protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.OldItems)
                {
                    item.PropertyChanged -= this.ChildPropertyChanged;
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (T item in e.NewItems)
                {
                    item.PropertyChanged += this.ChildPropertyChanged;
                }
            }

            base.OnCollectionChanged(e);
        }

        protected void OnItemPropertyChanged(ItemPropertyChangedEventArgs e)
        {
            this.ItemPropertyChanged?.Invoke(this, e);
        }

        protected void OnItemPropertyChanged(int index, PropertyChangedEventArgs e)
        {
            this.OnItemPropertyChanged(new ItemPropertyChangedEventArgs(index, e));
        }

        private void ChildPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var typedSender = (T)sender;
            var i = this.Items.IndexOf(typedSender);

            if (i < 0)
            {
                throw new ArgumentException("Received property notification from item not in collection");
            }

            this.OnItemPropertyChanged(i, e);
        }

        private void ObserveAll()
        {
            foreach (var item in this.Items)
            {
                item.PropertyChanged += this.ChildPropertyChanged;
            }
        }
    }

    /// <summary>
    ///     Provides data for the <see cref="FullyObservableCollection{T}.ItemPropertyChanged" /> event.
    /// </summary>
    public class ItemPropertyChangedEventArgs : PropertyChangedEventArgs
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemPropertyChangedEventArgs" /> class.
        /// </summary>
        /// <param name="index">The index in the collection of changed item.</param>
        /// <param name="name">The name of the property that changed.</param>
        public ItemPropertyChangedEventArgs(int index, string name)
            : base(name)
        {
            this.CollectionIndex = index;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ItemPropertyChangedEventArgs" /> class.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="args">The <see cref="PropertyChangedEventArgs" /> instance containing the event data.</param>
        public ItemPropertyChangedEventArgs(int index, PropertyChangedEventArgs args)
            : this(index, args.PropertyName)
        {
        }

        /// <summary>
        ///     Gets the index in the collection for which the property change has occurred.
        /// </summary>
        /// <value>
        ///     Index in parent collection.
        /// </value>
        public int CollectionIndex { get; }
    }
}