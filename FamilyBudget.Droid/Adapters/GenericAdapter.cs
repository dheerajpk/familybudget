using Android.App;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FamilyBudget.Droid.Adapters
{
    public class GenericAdapter<T> : BaseAdapter<T> where T : class
    {
       private IList<T> _items;
       private int _resourceId;
       private Activity _activity;

        public Action<Android.Views.View, T> OnRowLayoutBind;


        public override int Count
        {
            get { return Items?.Count ?? 0; }
        }

        protected IList<T> Items
        {
            get
            {
                return _items;
            }

            set
            {
                _items = value;
            }
        }

        protected int ResourceID
        {
            get
            {
                return _resourceId;
            }

            set
            {
                _resourceId = value;
            }
        }

        protected Activity Activity
        {
            get
            {
                return _activity;
            }

            set
            {
                _activity = value;
            }
        }


        public override T this[int position]
        {
            get
            {
                return Items[position];
            }
        }

        public override Java.Lang.Object GetItem(int position)
        {
            return new Java.Lang.String(Items[position].ToString());
        }

        public override long GetItemId(int position)
        {
            return Items[position].GetHashCode();
        }


        protected GenericAdapter()
        {
            this._items = new List<T>();
        }

        public GenericAdapter(IList<T> itemsource, int layoutresourceid, Activity activity, Action<Android.Views.View, T> rowlayoutbind)
        {
            Items = itemsource;
            ResourceID = layoutresourceid;
            Activity = activity;
            OnRowLayoutBind = rowlayoutbind;
        }

        public override Android.Views.View GetView(int position, Android.Views.View convertView, Android.Views.ViewGroup parent)
        {
            convertView = Activity.LayoutInflater.Inflate(ResourceID, parent, false);
            OnRowLayoutBind?.Invoke(convertView, Items[position]);
            return convertView;
        }
    }

    public class GroupedGenericAdapter<T, T1> : GenericAdapter<T>
        where T : class where T1 : Grouping<string, T>
        //GenericAdapter<T> where T : class
    {

        IList<T1> items;
        List<int> headerIndex = new List<int>();
        private int headerResourceId;
        private Action<Android.Views.View, T> OnHeaderLayoutBind;


        //public override int Count
        //{
        //    get { return Items == null ? 0 : GetTotalRowCount(); }
        //}

        private GroupedGenericAdapter()
        {
            base.Items = new List<T>();
            //var jj = new GroupedGenericAdapter<Models.Job, Grouping<string, Models.Job>>();
        }

        public GroupedGenericAdapter(IList<T1> itemsource, int headerlayoutresourceid, int rowlayoutresourceid, Activity activity, Action<Android.Views.View, T> headerlayoutbind, Action<Android.Views.View, T> rowlayoutbind) : base()
        {
            base.ResourceID = rowlayoutresourceid;
            base.OnRowLayoutBind = rowlayoutbind;
            base.Activity = activity;
            headerResourceId = headerlayoutresourceid;
            OnHeaderLayoutBind = headerlayoutbind;
            items = itemsource;
            AddItems();
        }

        private void AddItems()
        {
            int indx = 0;
            foreach (var item in items)
            {
                headerIndex.Add(indx++);
                base.Items.Add(item.FirstOrDefault());
                foreach (var itemrw in item)
                {
                    base.Items.Add(itemrw);
                    indx++;
                }
            }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (headerIndex.Contains(position))
            {
                convertView = Activity.LayoutInflater.Inflate(headerResourceId, parent, false);
                if (null != OnHeaderLayoutBind)
                {
                    OnHeaderLayoutBind(convertView, Items[position]);
                }
                return convertView;
            }

            return base.GetView(position, convertView, parent);
        }
    }

    public class Grouping<K, T> : List<T>
    {
        public K Key { get; private set; }

        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Add(item);
        }
    }
}