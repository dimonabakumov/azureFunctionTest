#load "Services\CreateMe.csx"

using System;

public class SlackInterpreter
{
	public object ExtractCommand(string request)
	{
	    try
	    {
	        var command = request.Split(' ')[1];
	        var digitCommand = int.Parse(command);

	        switch (digitCommand)
	        {
                case 1:
                    return new CreateMe().Auth();
                    break;

                //case 2:
                //    return 2;
                //    break;

                //case 3:
                //    return 3;
                //    break;

                default:
                    return new { text = "def" };
                    break;
            }
	    }
	    catch (Exception ex)
	    {
            return new { exeprion = ex };
            //return BadRequest();
            //return 69;
            //return "Can't parse this";
        }
	}
}
