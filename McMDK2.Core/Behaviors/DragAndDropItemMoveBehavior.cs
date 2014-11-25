using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;
using System.Windows.Media;

#pragma warning disable 1591

namespace McMDK2.Core.Behaviors
{
    /// <summary>
    /// http://nine-works.blog.ocn.ne.jp/blog/2012/10/listbox_408f.html
    /// 上記ソースのままだと、単純なクリック処理でもドラッグアンドドロップが発生しうるので、
    /// 単純なクリック処理では発生しないように改変。
    /// </summary>
    public class DragAndDropItemMoveBehavior : Behavior<ListBox>
    {
        private int MoveItemIndex { set; get; }

        private int InsertItemIndex { set; get; }

        private Point StartPoint { set; get; }

        public IList TargetCollection
        {
            get
            {
                return (IList)this.GetValue(TargetCollectionProperty);
            }
            set
            {
                this.SetValue(TargetCollectionProperty, value);
            }
        }

        public static readonly DependencyProperty TargetCollectionProperty = DependencyProperty.Register("TargetCollection", typeof(IList), typeof(DragAndDropItemMoveBehavior), new PropertyMetadata(null));

        protected override void OnAttached()
        {
            base.OnAttached();

            this.AssociatedObject.PreviewMouseLeftButtonDown += OnPreviewMouseLeftButtonDown;
            this.AssociatedObject.MouseMove += OnMouseMove;
            this.AssociatedObject.Drop += OnDrop;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();

            this.AssociatedObject.PreviewMouseLeftButtonDown -= OnPreviewMouseLeftButtonDown;
            this.AssociatedObject.MouseMove -= OnMouseMove;
            this.AssociatedObject.Drop -= OnDrop;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.StartPoint = e.GetPosition(this.AssociatedObject);
        }

        private void OnMouseMove(object sener, MouseEventArgs e)
        {
            Point movePos = e.GetPosition(this.AssociatedObject);
            Vector vector = this.StartPoint - movePos;
            if (e.LeftButton == MouseButtonState.Pressed && (Math.Abs(vector.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(vector.Y) > SystemParameters.MinimumVerticalDragDistance))
            {
                this.MoveItemIndex = this.GetItemIndex(e.GetPosition(this.AssociatedObject));
                if (this.MoveItemIndex != -1)
                {
                    this.AssociatedObject.AllowDrop = true;
                    DragDrop.DoDragDrop(this.AssociatedObject, this.MoveItemIndex, DragDropEffects.Move);
                }
            }
        }

        private void OnDrop(object sender, DragEventArgs e)
        {
            this.InsertItemIndex = this.GetItemIndex(e.GetPosition(this.AssociatedObject));
            if (this.InsertItemIndex != -1 && this.MoveItemIndex != -1 && this.InsertItemIndex != this.MoveItemIndex)
            {
                var moveItem = this.TargetCollection[this.MoveItemIndex];
                this.TargetCollection.Remove(moveItem);
                this.TargetCollection.Insert(this.InsertItemIndex, moveItem);
            }
            this.AssociatedObject.AllowDrop = false;
        }

        private int GetItemIndex(Point point)
        {
            var result = VisualTreeHelper.HitTest(this.AssociatedObject, point);
            if (result != null)
            {
                var item = result.VisualHit;
                while (item != null)
                {
                    if (item is ListBoxItem)
                        break;

                    item = VisualTreeHelper.GetParent(item);
                }

                if (item != null)
                {
                    return this.AssociatedObject.Items.IndexOf(((ListBoxItem)item).Content);
                }

            }
            return -1;
        }
    }
}
