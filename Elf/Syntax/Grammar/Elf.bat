@echo off

echo Cleaning up the environment
echo ============================
echo.
del ElfLexer.cs
del ElfParser.cs
del Elf.tokens

echo.
echo Compiling lexer and parser
echo ============================
echo.
java org.antlr.Tool Elf.g

echo.
pause