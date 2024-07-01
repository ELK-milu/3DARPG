/* FlexibleGridLayout.cs
* From: Game Dev Guide - Fixing Grid Layouts in Unity With a Flexible Grid Component
* Created: June 2020, NowWeWake
*/

using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class FlexibleGridLayout : LayoutGroup
{
    public enum FitType
    {
        Uniform,
        Width,
        Height,
        FixedRows,
        FixedColumns
    }
    [Header("Flexible Grid")]
    public FitType fitType = FitType.Uniform;

    public int rows;
    public int columns;
    public int Pages;
    public Vector2 cellSize;
    public Vector2 spacing;
    public bool fitX;
    public bool fitY;
    [Header("父实体Rect自适应")]
    public ParentFitProperties ParentAdaption;
    [System.Serializable]
    public class ParentFitProperties
    {
        [Header("开启自适应")]
        public bool ParentFit;
        [Header("按照页数自适应,仅在Fixed模式下生效")]
        public bool PageFit;
        [Header("每页行(列)数，详见代码注释")]
        // 这个数字决定了网格的子实体的增加模式,仅在Fixed模式下生效
        // 它代表了在与设定Fixed每增加多少个子实体进行一次换行(列)
        // 当为Fixed Rows且Rows=N,则每Row增加NumPerPage个子实体后会切换到下一Row
        // 直到增加完一个N行NumPerPage列为止
        public int NumPerPage = 1;
    }
    public override void CalculateLayoutInputHorizontal()
    {
        if (!ParentAdaption.ParentFit)
        {
            ParentAdaption.NumPerPage = 1;
        }
        // 调用基类方法以确保计算水平布局输入
        base.CalculateLayoutInputHorizontal();

        if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform)
        {
            // 计算子对象数量的平方根
            float squareRoot = Mathf.Sqrt(transform.childCount);
            // 向上取整得到行数和列数
            rows = columns = Mathf.CeilToInt(squareRoot);
            // 根据fitType选择布局方式
            switch (fitType)
            {
                case FitType.Width:
                    fitX = true;
                    fitY = false;
                    break;
                case FitType.Height:
                    fitX = false;
                    fitY = true;
                    break;
                case FitType.Uniform:
                    fitX = fitY = true;
                    break;
                case FitType.FixedColumns:
                    if (ParentAdaption.ParentFit)
                    {
                        fitX = true;
                        fitY = false;
                    }
                    break;
                case FitType.FixedRows:
                    if (ParentAdaption.ParentFit)
                    {
                        fitX = false;
                        fitY = true;
                    }
                    break;
            }
        }

        // 更新行数和列数
        if (fitType == FitType.Width || fitType == FitType.FixedColumns)
        {
            // 根据列数计算行数
            rows = Mathf.CeilToInt(transform.childCount / (float)columns);
        }
        if (fitType == FitType.Height || fitType == FitType.FixedRows)
        {
            // 根据行数计算列数
            columns = Mathf.CeilToInt(transform.childCount / (float)rows);
        }
        float parentWidth = rectTransform.rect.width;
        float parentHeight = rectTransform.rect.height;
        float cellWidth = parentWidth / (float)columns - ((spacing.x / (float)columns) * (columns - 1))
            - (padding.left / (float)columns) - (padding.right / (float)columns);
        float cellHeight = parentHeight / (float)rows - ((spacing.y / (float)rows) * (rows - 1))
            - (padding.top / (float)rows) - (padding.bottom / (float)rows);
        // 如果fitX为true，则使用计算的单元格宽度，否则使用cellSize.x
        // cellSize.x = fitX ? cellWidth : cellSize.x;
        // 如果fitY为true，则使用计算的单元格高度，否则使用cellSize.y
        // cellSize.y = fitY ? cellHeight : cellSize.y;
        // 当前列的索引
        int columnCount = 0;
        // 当前行的索引
        int rowCount = 0;
        if (ParentAdaption.PageFit)
        {
            if (fitType == FitType.FixedRows)
            {
                Pages = (columns + ParentAdaption.NumPerPage - 1) / ParentAdaption.NumPerPage;
            }
            else if (fitType == FitType.FixedColumns)
            {
                Pages = (rows + ParentAdaption.NumPerPage - 1) / ParentAdaption.NumPerPage;
            }

        }
        // 每页子实体数量
        int ChildPerPage = ParentAdaption.NumPerPage * rows;
        for (int i = 0; i < rectChildren.Count; i++)
        {
            // 根据fitType决定计算行索引和列索引的方式
            if (fitType == FitType.FixedRows)
            {
                // 页面增量
                int pageAddition = i / ChildPerPage * ParentAdaption.NumPerPage;
                // 行索引基值
                int baseColumn = i % ParentAdaption.NumPerPage;
                // 列索引基值
                int baseRow = i / ParentAdaption.NumPerPage;
                // 计算当前子对象所在的行索引
                rowCount = baseRow % rows;
                // 计算当前子对象所在的列索引
                columnCount = baseColumn + pageAddition;
            }
            else if (fitType == FitType.FixedColumns)
            {
                // 计算当前子对象所在的行索引
                rowCount = i / columns;
                // 计算当前子对象所在的列索引
                columnCount = i % columns;
            }
            // 获取子对象
            var item = rectChildren[i];
            // 计算子对象的x位置
            var xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
            // 计算子对象的y位置
            var yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;
            // 设置子对象在x轴上的位置和大小
            SetChildAlongAxis(item, 0, xPos, cellSize.x);
            // 设置子对象在y轴上的位置和大小
            SetChildAlongAxis(item, 1, yPos, cellSize.y);
            // 随着子对象的增加，父对象也应当自适应增加宽高
            if (ParentAdaption.ParentFit)
            {
                // 父对象的宽度和高度
                parentWidth = (cellSize.x * columns) + (spacing.x * (columns - 1)) + padding.left + padding.right;
                parentHeight = (cellSize.y * rows) + (spacing.y * (rows - 1)) + padding.top + padding.bottom;
                // 设置父对象的大小
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentHeight);
            }
        }
        if (ParentAdaption.ParentFit)
        {
            // 如果开启页自适应模式,会以页数来进行父对象的尺寸增加
            if (ParentAdaption.PageFit)
            {
                if (fitType == FitType.FixedRows)
                {
                    // 父对象的宽度和高度
                    parentWidth = (cellSize.x * (Pages * ParentAdaption.NumPerPage)) + (spacing.x * ((Pages * ParentAdaption.NumPerPage) - 1)) + padding.left + padding.right;
                    parentHeight = (cellSize.y * rows) + (spacing.y * (rows - 1)) + padding.top + padding.bottom;
                }
                else if (fitType == FitType.FixedColumns)
                {
                    // 父对象的宽度和高度
                    parentWidth = (cellSize.x * columns) + (spacing.x * (columns - 1)) + padding.left + padding.right;
                    parentHeight = (cellSize.y * (Pages * ParentAdaption.NumPerPage)) + (spacing.y * ((Pages * ParentAdaption.NumPerPage) - 1)) + padding.top + padding.bottom;
                }
                // 设置父对象的大小
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth);
                rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, parentHeight);
            }
        }
    }

    public override void CalculateLayoutInputVertical()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutHorizontal()
    {
        //throw new System.NotImplementedException();
    }

    public override void SetLayoutVertical()
    {
        //throw new System.NotImplementedException();
    }
}