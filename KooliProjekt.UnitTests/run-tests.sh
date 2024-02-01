TEST_DIR=TestResults
UNIT_TEST_DIR=$TEST_DIR/UnitTests
HTML_OUTPUT_DIR=$TEST_DIR/Coverage

dotnet test --collect "XPlat Code Coverage" --results-directory $UNIT_TEST_DIR

coverage_id=$(ls -Art TestResults/UnitTests | tail -n 1)
mv $UNIT_TEST_DIR/$coverage_id/coverage.cobertura.xml $UNIT_TEST_DIR/coverage.cobertura.xml
rm -r $UNIT_TEST_DIR/$coverage_id

reportgenerator "-reports:$UNIT_TEST_DIR/coverage.cobertura.xml" "-targetdir:$HTML_OUTPUT_DIR" "-reporttype:Html" "-classfilters:-AspNetCoreGeneratedDocument.*"

open $HTML_OUTPUT_DIR/index.html
