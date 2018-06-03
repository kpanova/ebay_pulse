#!/bin/bash -e

export OPENSSL_CONF=localhost.cnf
openssl req -out localhostCsr.pem -newkey rsa:2048 -keyout localhostPrivateKey.pem -nodes

export OPENSSL_CONF=../../CertificateAuthority/caconf.cnf
openssl ca -in localhostCsr.pem -out localhostCertificate.pem
openssl pkcs12 -export -in localhostCertificate.pem -inkey localhostPrivateKey.pem -out localhostCertificate.pfx
