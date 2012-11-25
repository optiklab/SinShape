using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace SinShape
{
    #region Description
    ///<summary>
    /// Description
    ///</summary>
    public class SinCanvas : Canvas
    {
        #region Public methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visual"></param>
        public void AddVisual(Visual visual)
        {
            _visuals.Add(visual);
            base.AddVisualChild(visual);
            base.AddLogicalChild(visual);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="visual"></param>
        public void DeleteVisual(Visual visual)
        {
            _visuals.Remove(visual);
            base.RemoveVisualChild(visual);
            base.RemoveLogicalChild(visual);
        }

        #endregion

        #region Protected methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        protected override Visual GetVisualChild(int index)
        {
            return _visuals[index];
        }

        /// <summary>
        /// 
        /// </summary>
        protected override int VisualChildrenCount
        {
            get
            {
                return _visuals.Count;
            }
        }

        #endregion

        #region Private fields

        /// <summary>
        /// 
        /// </summary>
        private List<Visual> _visuals = new List<Visual>();

        #endregion
    }

    #endregion
}
