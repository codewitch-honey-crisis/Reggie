EXEC [dbo].[Example_MatchCommentBlock] @value= N'foo  /* test //*  bar  /* baz *//* */ fubar'
EXEC [dbo].[Example_MatchWhitespace] @value= N'foo  /* test //*  bar  /* baz *//* */ fubar'
GO