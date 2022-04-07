namespace MatrixForm
{
    class Controller
    {
        public Controller(Model model)
        {
            Model = model;
        }

        private Model Model { get; set; }

        internal void Calculate(ActionEnum action, Matrix matrix1, Matrix matrix2)
        {
            switch (action)
            {
                case ActionEnum.MultMatrix:
                    Model.MultMatrix(matrix1, matrix2);
                    break;

                case ActionEnum.ReverseMatrix:
                    Model.ReverseMatrix(matrix1);
                    break;
            }
        }

        
    }
}
