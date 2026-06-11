# Performance Results

Representative compatibility routes are deterministic, local, and synthetic-only. Current performance coverage executes in-process through `WebApplicationFactory` for HTTP/FHIR and SOAP/XML routes, with HL7 ACK generation validated directly through the MLLP service.

## SC-003 Checks

- HTTP/FHIR and SOAP/XML: `Native_http_and_soap_p95_response_is_under_one_second` samples Epic launch, Epic FHIR patient read, and Unity SOAP `Magic` 10 times each. Assertion: p95 under 1000 ms. Result: passed.
- HL7 MLLP: `Hl7_ack_generation_p95_response_is_under_one_second` samples ACK generation 30 times. Assertion: p95 under 1000 ms. Result: passed.
- Existing provider route baseline: `Provider_route_average_response_is_under_one_second` samples provider search 10 times. Assertion: average under 1000 ms. Result: passed.

No external EMR, connector bridge, or network service is required for these checks.