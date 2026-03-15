import hashlib
import json
from time import time
from flask import Flask, jsonify, request
from uuid import uuid4

class Blockchain:
    def __init__(self):
        self.chain = []
        self.currentTransactions = []
        
        # Default
        self.NewBlock(previousHash='1', proof=100)

    def NewBlock(self, proof, previousHash=None):
        block = {
            'index': len(self.chain) + 1,
            'timestamp': time(),
            'transactions': self.currentTransactions,
            'proof': proof,
            'previous_hash': previousHash or self.Hash(self.chain[-1]),
        }

        self.currentTransactions = []
        self.chain.append(block)
        return block

    def NewTransaction(self, sender, recipient, amount):
        self.currentTransactions.append({
            'sender': sender,
            'recipient': recipient,
            'amount': amount,
        })
        
        return self.lastBlock['index'] + 1

    @staticmethod
    def Hash(block):
        return hashlib.sha256(json.dumps(block, sort_keys=True).encode()).hexdigest()

    @property
    def lastBlock(self):
        return self.chain[-1]

    def ProofOfWork(self, lastProof):
        proof = 0
        while self.ValidProof(lastProof, proof) is False:
            proof += 1
        return proof

    @staticmethod
    def ValidProof(lastProof, proof):
        hash = hashlib.sha256(f'{lastProof}{proof}'.encode()).hexdigest()
        return hash[:4] == "0000"


# --- REST API ---

app = Flask(__name__)

nodeId = str(uuid4()).replace('-', '')

blockchain = Blockchain()

@app.route('/mine', methods=['GET'])
def mine():
    lastBlock = blockchain.lastBlock

    blockchain.NewTransaction(
        sender="0",
        recipient=nodeId,
        amount=1,
    )

    block = blockchain.NewBlock(blockchain.ProofOfWork(lastBlock['proof']), blockchain.Hash(lastBlock))

    response = {
        'message': "Novy blok byl vytezen",
        'index': block['index'],
        'transactions': block['transactions'],
        'proof': block['proof'],
        'previous_hash': block['previous_hash'],
    }
    return jsonify(response), 200

@app.route('/transactions/new', methods=['POST'])
def new_transaction():
    values = request.get_json()

    required = ['sender', 'recipient', 'amount']
    if not values or not all(k in values for k in required):
        return 'Chybi hodnoty v JSON', 400

    response = {'message': f'Transakce bude pridana do bloku {blockchain.NewTransaction(values['sender'], values['recipient'], values['amount'])}'}
    return jsonify(response), 201

@app.route('/chain', methods=['GET'])
def full_chain():
    response = {
        'chain': blockchain.chain,
        'length': len(blockchain.chain),
    }
    return jsonify(response), 200

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)