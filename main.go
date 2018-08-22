package main

import (
	"database/sql"
	"fmt"
	"io/ioutil"
	"log"
	"net/http"
	"os"
	"os/exec"
	"strconv"
	"strings"

	_ "github.com/denisenkom/go-mssqldb"
)

func main() {
	//SQL Login
	server := "localhost"
	port := "1433"
	user := "invadmin"
	password := "invadmin"
	database := "CHEMINVDB2"

	input_value := ""
	fmt.Print("INVENTORY SQL ADDRESS:" + server + ">")
	fmt.Scanln(&input_value)
	if input_value != "" {
		server = input_value
	}

	input_value = ""
	fmt.Print("INVENTORY SQL Port:" + port + ">")
	fmt.Scanln(&input_value)
	if input_value != "" {
		port = input_value
	}

	input_value = ""
	fmt.Print("INVENTORY Username:" + user + ">")
	fmt.Scanln(&input_value)
	if input_value != "" {
		user = input_value
	}

	input_value = ""
	fmt.Print("INVENTORY password:" + password + ">")
	fmt.Scanln(&input_value)
	if input_value != "" {
		password = input_value
	}

	input_value = ""
	fmt.Print("INVENTORY database name:" + database + ">")
	fmt.Scanln(&input_value)
	if input_value != "" {
		database = input_value
	}

	int_port, err := strconv.Atoi(port)
	if err != nil {
		log.Fatal("illegal port:", err.Error())
		os.Exit(0)
	}

	connString := fmt.Sprintf("server=%s;port=%d;database=%s;user id=%s;password=%s", server, int_port, database, user, password)
	fmt.Println(connString)
	conn, err := sql.Open("mssql", connString)
	if err != nil {
		log.Fatal("Open Connection failed:", err.Error())
		os.Exit(0)
	}
	for true {
		fmt.Println("----------")
		CAS := ""
		fmt.Print("CAS Number:Leave Empty to quit>")
		fmt.Scanln(&input_value)
		if input_value != "" {
			CAS = input_value
		} else {
			os.Exit(0)
		}

		urladdr := "https://www.ncbi.nlm.nih.gov/pccompound?term=" + CAS
		resp, err := http.Get(urladdr)
		if err != nil {
			log.Fatal(err)
		}
		bytebody, err := ioutil.ReadAll(resp.Body)
		if err != nil {
			log.Fatal(err)
		}
		defer resp.Body.Close()
		content := string(bytebody)

		SMILES := ""
		SUBSTANCE_NAME := ""
		MOLECULAR_WEIGHT := ""
		MOLECULAR_FORMULA := ""
		if strings.Contains(content, " <meta property=\"og:url\" content=\"https://pubchem.ncbi.nlm.nih.gov/compound/") {
			CID := strings.Split(content, " <meta property=\"og:url\" content=\"https://pubchem.ncbi.nlm.nih.gov/compound/")[1]
			CID = strings.Split(CID, "\"")[0]

			urladdr = "https://pubchem.ncbi.nlm.nih.gov/rest/pug_view/data/compound/" + CID + "/JSON"
			resp, err := http.Get(urladdr)
			if err != nil {
				log.Fatal(err)
			}
			bytebody, err := ioutil.ReadAll(resp.Body)
			if err != nil {
				log.Fatal(err)
			}
			defer resp.Body.Close()
			content := string(bytebody)

			SMILES = strings.Split(content, "\"Name\": \"Canonical SMILES\",")[1]
			SMILES = strings.Split(SMILES, "\"StringValue\": \"")[1]
			SMILES = strings.Split(SMILES, "\"")[0]

			if strings.Contains(content, "Primary Identifying Name") {
				SUBSTANCE_NAME = strings.Split(content, "Primary Identifying Name")[1]
				SUBSTANCE_NAME = strings.Split(SUBSTANCE_NAME, "\"StringValue\": \"")[1]
				SUBSTANCE_NAME = strings.Split(SUBSTANCE_NAME, "\"")[0]
			}

			if strings.Contains(content, "Molecular Weight") {
				MOLECULAR_WEIGHT = strings.Split(content, "Molecular Weight")[1]
				MOLECULAR_WEIGHT = strings.Split(MOLECULAR_WEIGHT, "\"NumValue\": ")[1]
				MOLECULAR_WEIGHT = strings.Split(MOLECULAR_WEIGHT, ",")[0]
			}

			if strings.Contains(content, "\"Name\": \"Molecular Formula\",") {
				MOLECULAR_FORMULA = strings.Split(content, "\"Name\": \"Molecular Formula\",")[1]
				MOLECULAR_FORMULA = strings.Split(MOLECULAR_FORMULA, "\"StringValue\": \"")[1]
				MOLECULAR_FORMULA = strings.Split(MOLECULAR_FORMULA, "\"")[0]
			}

		}
		fmt.Print("SMILES:" + SMILES + ">")
		fmt.Scanln(&input_value)
		if input_value != "" {
			SMILES = input_value
		}

		ioutil.WriteFile("smiles.input", []byte(SMILES), 0644)
		cmd := exec.Command("convert.exe")
		err = cmd.Run()
		if err != nil {
			log.Fatal(err)
		}
		bytebody, err = ioutil.ReadFile("cdx.output")
		if err != nil {
			log.Fatal(err)
		}
		content = string(bytebody)
		CDX := content

		if SUBSTANCE_NAME == "" {
			bytebody, err = ioutil.ReadFile("name.output")
			if err != nil {
				log.Fatal(err)
			}
			content = string(bytebody)
			SUBSTANCE_NAME = content
		}

		if MOLECULAR_WEIGHT == "" {
			bytebody, err = ioutil.ReadFile("weight.output")
			if err != nil {
				log.Fatal(err)
			}
			content = string(bytebody)
			MOLECULAR_WEIGHT = content
		}

		if MOLECULAR_FORMULA == "" {
			bytebody, err = ioutil.ReadFile("formula.output")
			if err != nil {
				log.Fatal(err)
			}
			content = string(bytebody)
			MOLECULAR_FORMULA = content
		}

		input_value = ""
		fmt.Print("SUBSTANCE NAME:" + SUBSTANCE_NAME + ">")
		fmt.Scanln(&input_value)
		if input_value != "" {
			SUBSTANCE_NAME = input_value
		}

		SQL := "INSERT INTO INV_COMPOUNDS (\"CAS\", \"SUBSTANCE_NAME\", \"BASE64_CDX\", \"MOLECULAR_WEIGHT\", \"MOLECULAR_FORMULA\") VALUES ('" + CAS + "', '" + SUBSTANCE_NAME + "', '" + CDX + "', '" + MOLECULAR_WEIGHT + "', '" + MOLECULAR_FORMULA + "');"
		conn.Exec(SQL)
	}
	defer conn.Close()
}
