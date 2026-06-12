import requests
import uuid
import random
import hashlib
from datetime import datetime

API_URL = "http://localhost:5000/api/agent/full-gathering"

def random_mac():
    return "02:00:00:%02x:%02x:%02x" % (
        random.randint(0, 255),
        random.randint(0, 255),
        random.randint(0, 255),
    )


def fake_sha256(text):
    return hashlib.sha256(text.encode()).hexdigest()


def generate_file_entries():
    files = [
        r"C:\\Users\\Public\\doc.txt",
    ]

    result = []

    for f in files:
        result.append({
            "path": f,
            "name": f.split("\\")[-1],
            "extension": f.split(".")[-1] if "." in f else "",
            "sizeBytes": random.randint(1000, 500000),
            "sha256": fake_sha256(f),
            "lastWriteTimeUtc": datetime.utcnow().isoformat(),
            "lastSeenUtc": datetime.utcnow().isoformat(),
            "createdTimeUtc": datetime.utcnow().isoformat(),
            "lastAccessTimeUtc": datetime.utcnow().isoformat(),
            "isReadOnly": False,
            "attributes": "Archive"
        })

    return result


def build_payload():
    return {
        "computer": {
            "hostname": f"DESKTOP-{uuid.uuid4().hex[:6]}",
            "fqdn": None,
            "domainName": "WORKGROUP",
            "ipv4Address": "?",
            "macAddress": random_mac(),
            "operatingSystem": "Windows 11 Pro",
            "agentVersion": "0.1-sim",
            "firstSeenUtc": datetime.utcnow().isoformat(),
            "lastSeenUtc": datetime.utcnow().isoformat()
        },
        "fileEntries": generate_file_entries()
    }


def send():
    payload = build_payload()

    response = requests.post(
        API_URL,
        json=payload,
        verify=False
    )

    print("Status:", response.status_code)
    print("Response:", response.text)


if __name__ == "__main__":
    send()