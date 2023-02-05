using osuTK;
using System;
using osu.Framework.Graphics;
using System.Collections.Generic;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
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

            foreach (Drawable d in Children)
            {
                if (d is IFilterable)
                {
                    osu.Framework.Logging.Logger.Log("d is a filterable!");
                    //filters.AddRange(((IFilterable)d).FilterTerms);
                }
            }

            return filters;
        }
    }

    public partial class FilterableDrawSizePreservingFillContainer : DrawSizePreservingFillContainer, IFilterable
    {
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

            foreach (Drawable d in Children)
            {
                if (d is IFilterable)
                {
                    osu.Framework.Logging.Logger.Log("d is a filterable!");
                    //filters.AddRange(((IFilterable)d).FilterTerms);
                }
            }

            return filters;
        }
    }
}