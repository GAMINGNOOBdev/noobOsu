using osu.Framework.Graphics;
using System.Collections.Generic;
using osu.Framework.Graphics.Containers;
using osu.Framework.Localisation;

namespace noobOsu.Game.UI.Basic
{
    public partial class FilterableDrawable : CompositeDrawable, IFilterable
    {
        protected readonly List<LocalisableString> filterTerms = new List<LocalisableString>();

        public virtual bool MatchingFilter
        {
            set
            {
                if (value)
                    Show();
                else
                    Hide();
            }
        }
        public bool FilteringActive
        {
            set {}
        }

        public IEnumerable<LocalisableString> FilterTerms => filterTerms;
    }

    public partial class FilterableContainer : Container, IFilterable
    {
        protected readonly List<LocalisableString> filterTerms = new List<LocalisableString>();

        public virtual bool MatchingFilter
        {
            set
            {
                if (value)
                    Show();
                else
                    Hide();
            }
        }

        public virtual bool FilteringActive
        {
            set
            {
            }
        }

        public virtual IEnumerable<LocalisableString> FilterTerms => GetChildrenFilters();

        private IEnumerable<LocalisableString> GetChildrenFilters()
        {
            List<LocalisableString> filters = new List<LocalisableString>();
            filters.AddRange(filterTerms);

            foreach (Drawable d in Children)
            {
                if (d is IFilterable)
                {
                    filters.AddRange(((IFilterable)d).FilterTerms);
                }
            }

            return filters;
        }
    }

    public partial class FilterableFillFlowContainer : FillFlowContainer, IFilterable
    {
        protected readonly List<LocalisableString> filterTerms = new List<LocalisableString>();

        public virtual bool MatchingFilter
        {
            set
            {
                if (value)
                    Show();
                else
                    Hide();
            }
        }

        public virtual bool FilteringActive
        {
            set
            {
            }
        }

        public virtual IEnumerable<LocalisableString> FilterTerms => GetChildrenFilters();

        private IEnumerable<LocalisableString> GetChildrenFilters()
        {
            List<LocalisableString> filters = new List<LocalisableString>();
            filters.AddRange(filterTerms);

            foreach (Drawable d in Children)
            {
                if (d is IFilterable)
                {
                    filters.AddRange(((IFilterable)d).FilterTerms);
                }
            }

            return filters;
        }
    }

    public partial class FilterableDrawSizePreservingFillContainer : DrawSizePreservingFillContainer, IFilterable
    {
        protected readonly List<LocalisableString> filterTerms = new List<LocalisableString>();

        public virtual bool MatchingFilter
        {
            set
            {
                if (value)
                    Show();
                else
                    Hide();
            }
        }

        public virtual bool FilteringActive
        {
            set
            {
            }
        }

        public virtual IEnumerable<LocalisableString> FilterTerms => GetChildrenFilters();

        private IEnumerable<LocalisableString> GetChildrenFilters()
        {
            List<LocalisableString> filters = new List<LocalisableString>();
            filters.AddRange(filterTerms);

            foreach (Drawable d in Children)
            {
                if (d is IFilterable)
                {
                    filters.AddRange(((IFilterable)d).FilterTerms);
                }
            }

            return filters;
        }
    }

    public partial class FilterableBasicScrollContainer<T> : BasicScrollContainer<T>, IFilterable where T : Drawable
    {
        protected readonly List<LocalisableString> filterTerms = new List<LocalisableString>();

        public virtual bool MatchingFilter
        {
            set
            {
                osu.Framework.Logging.Logger.Log("matching filter: " + value);
                if (value)
                    Show();
                else
                    Hide();
            }
        }

        public virtual bool FilteringActive
        {
            set
            {
            }
        }

        public virtual IEnumerable<LocalisableString> FilterTerms => GetChildrenFilters();

        private IEnumerable<LocalisableString> GetChildrenFilters()
        {
            List<LocalisableString> filters = new List<LocalisableString>();
            filters.AddRange(filterTerms);

            foreach (Drawable d in Children)
            {
                if (d is IFilterable)
                {
                    filters.AddRange(((IFilterable)d).FilterTerms);
                }
            }

            return filters;
        }
    }
}