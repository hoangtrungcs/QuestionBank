/*
=========================================================================================================
  Module      : Enums (Enums.cs)
 -------------------------------------------------------------------------------------------------------
  Copyright   : Copyright T3. 2015 All Rights Reserved.
=========================================================================================================
*/

namespace EnglishQuestion.Common
{
    public enum QuestionLevel
    {
        Easy,
        Normal,
        Hard
    }

    public enum QuestionPurpose
    {
        All,
        Overview,
        Review,
        Test,
        Competition
    }

    public enum TestLevel
    {
        None,
        CLevelA,
        CLevelB,
        CLevelC,
        CLevelB1,
        CLevelB2,
        GLevelA,
        GLevelB,
        GLevelC,
        GLevelB1,
        GLevelB2,
        GcLevelA,
        GcLevelB,
        GcLevelC,
        GcLevelB1,
        GcLevelB2,
        SAudioFilePath,
        SB1B2
    }

    public enum LevelSection
    {
        None,
        #region Level A
        AR1,
        AR2,
        AR3,
        AR4A,
        AR4B,
        AL1,
        AL2,
        AL3,
        #endregion

        #region Level B
        BR1,
        BR2,
        BR3,
        BR4,
        BR5,
        BL1,
        BL2,
        BL3,
        #endregion

        #region Level C
        CR1,
        CR2,
        CW1A,
        CW1B,
        CW1C,
        CW2,
        CL1,
        CL2,
        CL3,
        #endregion

        #region Level B1
        B1R1,
        B1R2,
        B1R3,
        B1R4,
        B1R5,
        B1W1,
        B1W2,
        B1W3,
        B1L1,
        B1L2,
        B1L3,
        B1L4,
        #endregion

        #region Level B2
        B2R1,
        B2R2,
        B2R3,
        B2R4,
        B2R5,
        B2W1,
        B2W2,
        B2W3,
        B2L1,
        B2L2,
        B2L3,
        B2L4
        #endregion
    }

    public enum TestLevelType
    {
        A,B,C,B1,B2
    }

    public enum ActionType
    {
        FromDb, Insert, Modify
    }

    public enum QuestionType
    {
        // Listening Question and 4 answers
        LQA,
        // Listening question and 1 answer
        LQA1,
        RPA,
        RPAB1B2,
        RPQA,
        RPQAB1B2,
        RQA,
        WPA,
        WQA
    }

    public enum SubTestType
    {
        Writing,
        Listening
    }
}
