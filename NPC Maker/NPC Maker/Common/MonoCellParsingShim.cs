using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NPC_Maker.Common
{
    public class MonoCellParsingShim
    {
        private readonly DataGridView _grid;
        private readonly DataGridViewCellParsingEventHandler _parsing;
        private bool _inShim;
        private bool _nativeWorks;
        private int _suppress;

        // Run grid-population code without the shim reacting to CellValueChanged
        public void RunSilently(Action action)
        {
            _suppress++;
            try { action(); }
            finally { _suppress--; }
        }

        public MonoCellParsingShim(DataGridView grid, DataGridViewCellParsingEventHandler parsing)
        {
            _grid = grid;
            _parsing = parsing;
            grid.CellParsing += OnParsing;
            grid.CellValueChanged += OnValueChanged;
        }

        private void OnParsing(object s, DataGridViewCellParsingEventArgs e)
        {
            if (!_inShim) _nativeWorks = true;   // the real event fired
            _parsing(s, e);
        }

        private void OnValueChanged(object s, DataGridViewCellEventArgs e)
        {
            if (_nativeWorks || _inShim || _suppress > 0) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || e.RowIndex >= _grid.Rows.Count) return;

            var cell = _grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            _inShim = true;
            try
            {
                var args = new DataGridViewCellParsingEventArgs(
                    e.RowIndex, e.ColumnIndex, cell.Value ?? "", typeof(string), cell.InheritedStyle);
                _parsing(_grid, args);
                if (args.ParsingApplied && args.Value != null && !Equals(cell.Value, args.Value))
                    cell.Value = args.Value;
            }
            finally { _inShim = false; }
        }

        // For manual calls from double-click handlers etc.
        public void Parse(int row, int col, object value)
        {
            if (_grid.IsCurrentCellInEditMode) _grid.CancelEdit();

            bool prev = _inShim;
            _inShim = true;
            try
            {
                _parsing(_grid, new DataGridViewCellParsingEventArgs(
                    row, col, value, typeof(string), _grid.Rows[row].Cells[col].InheritedStyle));
            }
            finally { _inShim = prev; }

            _grid.InvalidateCell(col, row);
        }

        public void SetCell(int row, int col, object value)
        {
            bool prev = _inShim;
            _inShim = true;
            try { _grid.Rows[row].Cells[col].Value = value; }
            finally { _inShim = prev; }

            Parse(row, col, value);
        }
    }
}
