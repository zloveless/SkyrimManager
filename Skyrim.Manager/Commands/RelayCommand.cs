// ----------------------------------------------------------------
// Skyrim Manager
// Copyright (c) 2013. Zack Loveless.
// 
// Original author(s) for this source file: Zack Loveless
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
// ----------------------------------------------------------------

namespace Skyrim.Manager.Commands
{
	using System;
	using System.Windows.Input;

	public class RelayCommand : ICommand
	{
		private readonly Action<object> executeAction;
		private readonly Predicate<object> executeCondition;

		public RelayCommand(Action<object> executeAction, Predicate<object> executeCondition)
		{
			this.executeAction = executeAction;
			this.executeCondition = executeCondition;
		}

		#region Implementation of ICommand

		/// <summary>
		///     Defines the method that determines whether the command can execute in its current state.
		/// </summary>
		/// <returns>
		///     true if this command can be executed; otherwise, false.
		/// </returns>
		/// <param name="parameter">
		///     Data used by the command.  If the command does not require data to be passed, this object can
		///     be set to null.
		/// </param>
		public bool CanExecute(object parameter)
		{
			return executeCondition != null && executeCondition(parameter);
		}

		/// <summary>
		///     Defines the method to be called when the command is invoked.
		/// </summary>
		/// <param name="parameter">
		///     Data used by the command.  If the command does not require data to be passed, this object can
		///     be set to null.
		/// </param>
		public void Execute(object parameter)
		{
			if (executeAction != null) executeAction(parameter);
		}

		public event EventHandler CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}

		#endregion
	}
}
